let ceres = {};
(function(slideview)
{
    'use strict';

    slideview.HTMLSlideViewElement = 'ceres-slideview'; // required public element name and id
    slideview.HTMLImageListElement = 'ceres-csv'; // optional public markup noscript tag id when using embedded image lists

    window.customElements.define(slideview.HTMLSlideViewElement, class extends HTMLElement
    {
        async connectedCallback()
        {
            let src = this.getAttribute('src');
            this.innerHTML = await (await fetch(src)).text();

            let css = (this.getAttribute('css')) ? getBoolean(this.getAttribute('css')) : true;
            if (css) importSlideViewStylesheet();

            await this.renderComplete;

            initiateSlideView();
        }

    });

    slideview.openImageTab = function(el) { window.open(el.getAttribute('src'), 'image'); }; // public method reference
    slideview.getSlide = function(target, calc) { displaySlide(svc.index = (calc) ? svc.index += target : target); };  // public method reference

    const constants = {
        'defaultCSS': 'https://ceresbakalite.github.io/similarity/stylesheets/similaritysheetslide.css', // the default slideview stylesheet
        'notify': 1, // console notification type
        'error': 99 // console notification type
    };

    Object.freeze(constants);

    const manifest = {
        'LinkOnload': 100,
        'LinkAddEventListener': 101,
        'LinkStylesheetCount': 102,
        'LinkOnReadyState': 103,
        'ProgenitorInnerHTML': 104,
        'ImageListMarkup': 105,
        'ListFallback': 106,
        'NotFoundProgenitor': 107,
        'NotFoundListFallback': 108,
        'EmptyProgenitorSrc': 109
    };

    Object.freeze(manifest);

    let attributes = {
        'trace': false, // default element attribute - enable slideview trace environment directive
        'ptr': true, // default element attribute - display slideview item pointers
        'css': true, // default element attribute - use the default slideview stylesheet
        'sur': true, // default element attribute - display slideview item surtitles
        'sub': true  // default element attribute - display slideview item subtitles
    };

    let svc = {
        'progenitor': null,
        'imageArray': null,
        'index': 1
    }

/*
    let progenitor = null; // parent slideview place holder
    let imageArray = null; // slideview element item attributes array
    let index = 1; // pointer referencing to the currently active slide
*/
    function initiateSlideView()
    {
        svc.progenitor = (document.getElementById(slideview.HTMLSlideViewElement)) ? document.getElementById(slideview.HTMLSlideViewElement) : document.getElementsByTagName(slideview.HTMLSlideViewElement)[0];
        svc.imageArray = getSlideViewAttributes();

        if (svc.imageArray) activateSlideView();

        function getSlideViewAttributes()
        {
            if (svc.progenitor)
            {
                svc.progenitor.id = slideview.HTMLSlideViewElement;

                attributes.trace = (svc.progenitor.getAttribute('trace')) ? getBoolean(svc.progenitor.getAttribute('trace')) : attributes.trace;
                attributes.ptr = (svc.progenitor.getAttribute('ptr')) ? getBoolean(svc.progenitor.getAttribute('ptr')) : attributes.ptr;
                attributes.css = (svc.progenitor.getAttribute('css')) ? getBoolean(svc.progenitor.getAttribute('css')) : attributes.css;
                attributes.sur = (svc.progenitor.getAttribute('sur')) ? getBoolean(svc.progenitor.getAttribute('sur')) : attributes.sur;
                attributes.sub = (svc.progenitor.getAttribute('sub')) ? getBoolean(svc.progenitor.getAttribute('sub')) : attributes.sub;

                let imageList = getImageList();

                return (imageList) ? imageListToArray(imageList) : null;

            } else {

                return errorHandler(resource(constants.error, manifest.NotFoundProgenitor));

            }

            function imageListToArray(str)
            {
                if (!str) return null;

                if (attributes.trace) console.log(resource(constants.notify, manifest.ImageListMarkup, str));
                return str.replace(/((<([^>]+)>))/gi, '').trim().replace(/\r\n|\r|\n/gi, ';').split(';');
            }

            function getImageList()
            {
                return (svc.progenitor.getAttribute('src')) ? getMarkdownList() : getMarkupList();

                function getMarkdownList()
                {
                    return (svc.progenitor.innerHTML) ? svc.progenitor.innerHTML : null;
                }

                function getMarkupList()
                {
                    if (attributes.trace) console.log(resource(constants.notify, manifest.EmptyProgenitorSrc));

                    const lookup = {
                        'logFound': function() { if (attributes.trace) console.log(resource(constants.notify, manifest.ListFallback)); },
                        'logNotFound': function() { errorHandler(resource(constants.error, manifest.NotFoundListFallback)); },
                    };

                    const el = document.getElementById(slideview.HTMLImageListElement) ? document.getElementById(slideview.HTMLImageListElement) : document.getElementsByTagName('noscript')[0];
                    const list = (el) ? el.innerHTML : null;
                    const response = (list) ? 'logFound' : 'logNotFound';

                    lookup[response]();
                    return list;
                }

            }

        }

    }

    function getSlideView()
    {
        createSlideViewContainer();

        function createSlideViewContainer()
        {
            svc.progenitor.innerHTML = null;

            const imageContainer = document.createElement('div');

            imageContainer.id = slideview.HTMLSlideViewElement + '-image-container';
            svc.progenitor.appendChild(imageContainer);

            composeAttribute(imageContainer.id, 'class', 'slideview-image-container');

            for (let item = 0; item < svc.imageArray.length; item++)
            {
                var arrayItem = svc.imageArray[item].split(',');

                let qualifier = item + 1;
                let slideViewContainerId = 'slideview' + qualifier;

                let elements = {
                    'surName': 'slideview-sur' + qualifier,
                    'imgName': 'slideview-img' + qualifier,
                    'subName': 'slideview-sub' + qualifier
                };

                composeElement('div', slideViewContainerId, 'slideview fade', imageContainer, null, null, null, null);

                let slideViewContainer = document.getElementById(slideViewContainerId);

                if (attributes.sur) composeElement('div', elements.surName, 'surtitle', slideViewContainer, getSurtitle(qualifier), null, null, null);
                composeElement('img', elements.imgName, null, slideViewContainer, null, 'ceres.openImageTab(this);', getURL(), getAccessibilityText())
                if (attributes.sub) composeElement('div', elements.subName, 'subtitle', slideViewContainer, getSubtitle(), null, null, null);
            }

            composeElement('a', 'slideview-prev', 'prev', imageContainer, '&#10094;', 'ceres.getSlide(-1, true)', getURL(), null);
            composeElement('a', 'slideview-next', 'next', imageContainer, '&#10095;', 'ceres.getSlide(1, true)', getURL(), null);

            if (attributes.ptr) createSlideViewPointerContainer();

            setSlideViewDisplay('none');

            function createSlideViewPointerContainer()
            {
                svc.progenitor.appendChild(document.createElement('br'));

                const pointerElement = document.createElement('div');

                pointerElement.id = slideview.HTMLSlideViewElement + '-pointer-container';
                svc.progenitor.appendChild(pointerElement);

                composeAttribute(pointerElement.id, 'class', 'slideview-pointer-container');

                for (let item = 0; item < svc.imageArray.length; item++)
                {
                    let qualifier = item + 1;
                    let svpname = 'slideview-ptr' + qualifier;

                    composeElement('span', svpname, 'ptr', pointerElement, null, getClickEventValue(qualifier), null, null);
                }

                svc.progenitor.appendChild(document.createElement('br'));

                if (attributes.trace) console.log(resource(constants.notify, manifest.ProgenitorInnerHTML));

                function getClickEventValue(indexItem)
                {
                    return 'ceres.getSlide(' + indexItem + ')';
                }

            }

            function getURL()
            {
                return (arrayItem[0]) ? arrayItem[0].trim() : null;
            }

            function getSurtitle(indexItem)
            {
                return (attributes.sur) ? indexItem + ' / ' + svc.imageArray.length : null;
            }

            function getSubtitle()
            {
                return (attributes.sub) ? getAccessibilityText() : null;
            }

            function getAccessibilityText()
            {
                return (arrayItem[1]) ? arrayItem[1].trim() : null;
            }

        }

    }

    function importSlideViewStylesheet()
    {
        const link = document.createElement('link');

        link.rel = 'stylesheet';
        link.type = 'text/css';
        link.href = constants.defaultCSS;
        link.as = 'style';

        onloadListener();
        addEventListener();
        styleSheetsLengthListener();
        onReadyStateChangeListener();

        document.head.appendChild(link);

        function onloadListener()
        {
            link.onload = function ()
            {
                if (attributes.trace) console.log(resource(constants.notify, manifest.LinkOnload));
            }

        }

        function addEventListener()
        {
            if (link.addEventListener)
            {
                link.addEventListener('load', function()
                {
                    if (attributes.trace) console.log(resource(constants.notify, manifest.LinkAddEventListener));
                }, false);

            }

        }

        function styleSheetsLengthListener()
        {
            const cssnum = document.styleSheets.length;

            const ti = setInterval(function()
            {
                if (document.styleSheets.length > cssnum)
                {
                    clearInterval(ti);
                    if (attributes.trace) console.log(resource(constants.notify, manifest.LinkStylesheetCount));
                }

            }, 10);

        }

        function onReadyStateChangeListener()
        {
            link.onreadystatechange = function()
            {
                const state = link.readyState;

                if (state === 'loaded' || state === 'complete')
                {
                    link.onreadystatechange = null;
                    if (attributes.trace) console.log(resource(constants.notify, manifest.LinkOnReadyState));
                }

            };

        }

    }

    function displaySlide(targetIndex)
    {
        const slides = document.querySelectorAll(".slideview");
        const pointers = document.querySelectorAll(".ptr");

        svc.index = (targetIndex < 1) ? slides.length : (targetIndex > slides.length) ? 1 : svc.index;

        slides.forEach(node => { node.style.display = 'none'; } );
        slides[svc.index-1].style.display = 'block';

        if (attributes.ptr)
        {
            pointers.forEach(node => { node.className = node.className.replace(' active', ''); } );
            pointers[svc.index-1].className += ' active';
        }

    }

    function composeElement(element, id, classValue, parent, markup, onClickEventValue, url, accessibility)
    {
        const el = document.createElement(element);

        el.id = id;
        parent.appendChild(el);

        if (classValue) composeAttribute(el.id, 'class', classValue);
        if (onClickEventValue) composeAttribute(el.id, 'onclick', onClickEventValue);
        if (url) composeAttribute(el.id, 'src', url);
        if (accessibility) composeAttribute(el.id, 'alt', accessibility);
        if (markup) document.getElementById(el.id).innerHTML = markup;
    }

    function composeAttribute(id, type, value)
    {
        const el = document.getElementById(id);

        if (el)
        {
            const attribute = document.createAttribute(type);
            attribute.value = value;

            el.setAttributeNode(attribute);
        }

    }

    function activateSlideView()
    {
        const renderdelay = 250; // awaiting slideview css catchup

        svc.progenitor.style.display = 'none';

        getSlideView();
        displaySlide();

        setTimeout(function() { setSlideViewDisplay('block'); }, renderdelay);
    }


    function setSlideViewDisplay(attribute)
    {
        const nodelist = document.querySelectorAll('a.prev, a.next, div.subtitle, div.surtitle, #' + slideview.HTMLSlideViewElement);
        nodelist.forEach(node => { node.style.display = attribute; } );
    }

    function errorHandler(str)
    {
        const err = str + ' [ DateTime: ' + new Date().toLocaleString() + ' ]';
        console.log(err);

        if (attributes.trace) alert(err);

        return null;
    }

    function getBoolean(symbol)
    {
        const token = symbol.trim().toUpperCase();

        if (!token) return false;

        const lookup = {
            'TRUE': true,
            'T':  true,
            'YES': true,
            'Y': true,
            '1': true
        };

        return lookup[token] || false;
    }

    function resource(type, item, str)
    {
        const newline = '\n';

        const lookup = {
            [constants.notify]: function() { return lookupNotify(); },
            [constants.error]: function() { return lookupError(); },
            default: 'An unexpected error has occurred - ' + slideview.HTMLSlideViewElement + ' is unresponsive',
        };

        return lookup[type]() || lookup['default'];

        function lookupNotify()
        {
            const lookup = {
                [manifest.LinkOnload]: 'Link default stylesheet insert [' + slideview.HTMLSlideViewElement + ']: onload listener',
                [manifest.LinkAddEventListener]: 'Link default stylesheet insert [' + slideview.HTMLSlideViewElement + ']: addEventListener',
                [manifest.LinkStylesheetCount]: 'Link default stylesheet insert [' + slideview.HTMLSlideViewElement + ']: styleSheets.length increment',
                [manifest.LinkOnReadyState]: 'Link default stylesheet insert [' + slideview.HTMLSlideViewElement + ']: onreadystatechange event',
                [manifest.ProgenitorInnerHTML]: 'Progenitor innerHTML [' + slideview.HTMLSlideViewElement + ']: ' + newline + progenitor.innerHTML,
                [manifest.ImageListMarkup]: 'Image list markup [' + slideview.HTMLSlideViewElement + ']: ' + newline + str,
                [manifest.ListFallback]: 'Image list [' + slideview.HTMLImageListElement + ']: found on the second attempt in the element fallback location',
                [manifest.EmptyProgenitorSrc]: 'The ' + slideview.HTMLSlideViewElement + ' src attribute content is unavailable. Searching the fallback noscript image list content in the document body...',
                'default': 'An unexpected error has occurred - ' + slideview.HTMLSlideViewElement + ' trace notification is unresponsive'
            };

            return lookup[item] || lookup['default'];
        }

        function lookupError()
        {
            const lookup = {
                [manifest.NotFoundProgenitor]: 'Error: Unable to find the ' + slideview.HTMLSlideViewElement + ' document element',
                [manifest.NotFoundListFallback]: 'Error: Unable to find the ' + slideview.HTMLSlideViewElement + ' fallback noscript image list when searching the document body',
                'default': 'An unexpected error has occurred - ' + slideview.HTMLSlideViewElement + ' error notification is unresponsive'
            };

            return lookup[item] || lookup['default'];
        }

    }

})(ceres);
