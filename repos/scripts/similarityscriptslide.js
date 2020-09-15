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
    slideview.getSlide = function(target, calc) { displaySlide(csv.index = (calc) ? csv.index += target : target); };  // public method reference

    const constants = {
        'defaultCSS': 'https://ceresbakalite.github.io/similarity/stylesheets/similaritysheetslide.css', // the default slideview stylesheet
        'notify': 1, // console notification type
        'error': 99 // console notification type
    };

    Object.freeze(constants);

    const manifest = {
        'CSVObjectProperties': 100,
        'LinkOnload': 101,
        'LinkAddEventListener': 102,
        'LinkStylesheetCount': 103,
        'LinkOnReadyState': 104,
        'ProgenitorInnerHTML': 105,
        'ImageListMarkup': 106,
        'ListFallback': 107,
        'NotFoundProgenitor': 108,
        'NotFoundListFallback': 109,
        'EmptyProgenitorSrc': 110
    };

    Object.freeze(manifest);

    let csv = {
        'progenitor': null,
        'imageArray': null,
        'imageContainer': null,
        'slideContainer': null,
        'attributes': function() { return attribute; },
        'index': 1
    }

    Object.preventExtensions(csv);

    function initiateSlideView()
    {
        csv.progenitor = (document.getElementById(slideview.HTMLSlideViewElement)) ? document.getElementById(slideview.HTMLSlideViewElement) : document.getElementsByTagName(slideview.HTMLSlideViewElement)[0];
        csv.imageArray = getSlideViewAttributes();

        if (csv.imageArray) activateSlideView();

        function getSlideViewAttributes()
        {
            if (csv.progenitor)
            {
                csv.progenitor.id = slideview.HTMLSlideViewElement;

                csv.attributes.trace = (csv.progenitor.getAttribute('trace')) ? getBoolean(csv.progenitor.getAttribute('trace')) : false; // enable slideview trace environment directive
                csv.attributes.css = (csv.progenitor.getAttribute('css')) ? getBoolean(csv.progenitor.getAttribute('css')) : true; // use the default slideview stylesheet
                csv.attributes.ptr = (csv.progenitor.getAttribute('ptr')) ? getBoolean(csv.progenitor.getAttribute('ptr')) : true; // display slideview item pointers
                csv.attributes.sur = (csv.progenitor.getAttribute('sur')) ? getBoolean(csv.progenitor.getAttribute('sur')) : true; // display slideview item surtitles
                csv.attributes.sub = (csv.progenitor.getAttribute('sub')) ? getBoolean(csv.progenitor.getAttribute('sub')) : true; // display slideview item subtitles

                if (csv.attributes.trace) logAttributeProperties();

                let imageList = getImageList();

                return (imageList) ? imageListToArray(imageList) : null;

            } else {

                return errorHandler(resource(constants.error, manifest.NotFoundProgenitor));

            }

            function imageListToArray(str)
            {
                if (!str) return null;

                if (csv.attributes.trace) console.log(resource(constants.notify, manifest.ImageListMarkup, str));
                return str.replace(/((<([^>]+)>))/gi, '').trim().replace(/\r\n|\r|\n/gi, ';').split(';');
            }

            function getImageList()
            {
                return (csv.progenitor.getAttribute('src')) ? getMarkdownList() : getMarkupList();

                function getMarkdownList()
                {
                    return (csv.progenitor.innerHTML) ? csv.progenitor.innerHTML : null;
                }

                function getMarkupList()
                {
                    if (csv.attributes.trace) console.log(resource(constants.notify, manifest.EmptyProgenitorSrc));

                    const lookup = {
                        'logFound': function() { if (csv.attributes.trace) console.log(resource(constants.notify, manifest.ListFallback)); },
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
            csv.progenitor.innerHTML = null;

            csv.imageContainer = document.createElement('div');
            csv.imageContainer.id = slideview.HTMLSlideViewElement + '-image-container';
            csv.progenitor.appendChild(csv.imageContainer);

            composeAttribute(csv.imageContainer.id, 'class', 'slideview-image-container');

            for (let item = 0; item < csv.imageArray.length; item++)
            {
                var arrayItem = csv.imageArray[item].split(',');

                let qualifier = item + 1;
                let id = 'slideview' + qualifier;

                let elements = {
                    'surName': 'slideview-sur' + qualifier,
                    'imgName': 'slideview-img' + qualifier,
                    'subName': 'slideview-sub' + qualifier
                };

                composeElement('div', id, 'slideview fade', csv.imageContainer, null, null, null, null);

                csv.slideContainer = document.getElementById(id);

                if (csv.attributes.sur) composeElement('div', elements.surName, 'surtitle', csv.slideContainer, getSurtitle(qualifier), null, null, null);
                composeElement('img', elements.imgName, 'slide', csv.slideContainer, null, 'ceres.openImageTab(this);', getURL(), getAccessibilityText())
                if (csv.attributes.sub) composeElement('div', elements.subName, 'subtitle', csv.slideContainer, getSubtitle(), null, null, null);
            }

            composeElement('a', 'slideview-prev', 'prev', csv.imageContainer, '&#10094;', 'ceres.getSlide(-1, true)', getURL(), null);
            composeElement('a', 'slideview-next', 'next', csv.imageContainer, '&#10095;', 'ceres.getSlide(1, true)', getURL(), null);

            if (csv.attributes.ptr) createSlideViewPointerContainer();

            setSlideViewDisplay('none');

            if (csv.attributes.trace) console.log(resource(constants.notify, manifest.ProgenitorInnerHTML));

            function createSlideViewPointerContainer()
            {
                csv.progenitor.appendChild(document.createElement('br'));

                const pointerElement = document.createElement('div');

                pointerElement.id = slideview.HTMLSlideViewElement + '-pointer-container';
                csv.progenitor.appendChild(pointerElement);

                composeAttribute(pointerElement.id, 'class', 'slideview-pointer-container');

                for (let item = 0; item < csv.imageArray.length; item++)
                {
                    let qualifier = item + 1;
                    let svpname = 'slideview-ptr' + qualifier;

                    composeElement('span', svpname, 'ptr', pointerElement, null, getClickEventValue(qualifier), null, null);
                }

                csv.progenitor.appendChild(document.createElement('br'));

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
                return (csv.attributes.sur) ? indexItem + ' / ' + csv.imageArray.length : null;
            }

            function getSubtitle()
            {
                return (csv.attributes.sub) ? getAccessibilityText() : null;
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
                if (csv.attributes.trace) console.log(resource(constants.notify, manifest.LinkOnload));
            }

        }

        function addEventListener()
        {
            if (link.addEventListener)
            {
                link.addEventListener('load', function()
                {
                    if (csv.attributes.trace) console.log(resource(constants.notify, manifest.LinkAddEventListener));
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
                    if (csv.attributes.trace) console.log(resource(constants.notify, manifest.LinkStylesheetCount));
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
                    if (csv.attributes.trace) console.log(resource(constants.notify, manifest.LinkOnReadyState));
                }

            };

        }

    }

    function displaySlide(targetIndex)
    {
        const slides = document.querySelectorAll(".slideview");
        const pointers = document.querySelectorAll(".ptr");

        csv.index = (targetIndex < 1) ? slides.length : (targetIndex > slides.length) ? 1 : csv.index;

        slides.forEach(node => { node.style.display = 'none'; } );
        slides[csv.index-1].style.display = 'block';

        if (csv.attributes.ptr)
        {
            pointers.forEach(node => { node.className = node.className.replace(' active', ''); } );
            pointers[csv.index-1].className += ' active';
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

        csv.progenitor.style.display = 'none';

        getSlideView();
        displaySlide();

        setTimeout(function() { setSlideViewDisplay('block'); }, renderdelay);
    }


    function setSlideViewDisplay(attribute)
    {
        const nodelist = document.querySelectorAll('a.prev, a.next, div.subtitle, div.surtitle, img.slide, #' + slideview.HTMLSlideViewElement);
        nodelist.forEach(node => { node.style.display = attribute; } );
    }

    function errorHandler(str)
    {
        const err = str + ' [ DateTime: ' + new Date().toLocaleString() + ' ]';
        console.log(err);

        if (csv.attributes.trace) alert(err);

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

    function logAttributeProperties()
    {
        let str = '';
        for (let property in csv.attributes) str += property + ": " + csv.attributes[property] + ', ';
        console.log(resource(constants.notify, manifest.CSVObjectProperties, str));
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
                [manifest.CSVObjectProperties]: 'The Ceres Slide Viewer [' + slideview.HTMLSlideViewElement + '] object properties after initialisation: ' + str,
                [manifest.LinkOnload]: 'Link default stylesheet insert [' + slideview.HTMLSlideViewElement + ']: onload listener',
                [manifest.LinkAddEventListener]: 'Link default stylesheet insert [' + slideview.HTMLSlideViewElement + ']: addEventListener',
                [manifest.LinkStylesheetCount]: 'Link default stylesheet insert [' + slideview.HTMLSlideViewElement + ']: styleSheets.length increment',
                [manifest.LinkOnReadyState]: 'Link default stylesheet insert [' + slideview.HTMLSlideViewElement + ']: onreadystatechange event',
                [manifest.ProgenitorInnerHTML]: 'Progenitor innerHTML [' + slideview.HTMLSlideViewElement + ']: ' + newline + csv.progenitor.innerHTML,
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
