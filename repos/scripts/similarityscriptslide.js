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
            await this.renderComplete;
        }

    });

    slideview.openImageTab = function(el) { window.open(el.getAttribute('src'), 'image'); }; // public method reference
    slideview.getSlide = function(target, calc) { displaySlide(index = (calc) ? index += target : target); };  // public method reference

    const constants = {
        'defaultCSS': 'https://ceresbakalite.github.io/similarity/stylesheets/similaritysheetslide.css', // the default slideview stylesheet
        'renderdelay': 500, // onload setTimeout period in ms
        'notify': 1, // console notification type
        'error': 99 // console notification type
    };

    Object.freeze(constants);

    let progenitor = null; // parent slideview place holder
    let attributes = null; // slideview element item attributes array
    let trace = false; // default element attribute - enable slideview trace environment directive
    let ptr = true; // default element attribute - display slideview item pointers
    let css = true; // default element attribute - use the default slideview stylesheet
    let sur = true; // default element attribute - display slideview item surtitles
    let sub = true; // default element attribute - display slideview item subtitles
    let index = 1; // pointer referencing to the currently active slide

    loadSlideView();

    function initiateSlideView()
    {
        progenitor = (document.getElementById(slideview.HTMLSlideViewElement)) ? document.getElementById(slideview.HTMLSlideViewElement) : document.getElementsByTagName(slideview.HTMLSlideViewElement)[0];
        attributes = getSlideViewAttributes();

        if (attributes) activateSlideView();

        function getSlideViewAttributes()
        {
            if (progenitor)
            {
                progenitor.id = slideview.HTMLSlideViewElement;

                trace = (progenitor.getAttribute('trace')) ? getBoolean(progenitor.getAttribute('trace')) : trace;
                ptr = (progenitor.getAttribute('ptr')) ? getBoolean(progenitor.getAttribute('ptr')) : ptr;
                css = (progenitor.getAttribute('css')) ? getBoolean(progenitor.getAttribute('css')) : css;
                sur = (progenitor.getAttribute('sur')) ? getBoolean(progenitor.getAttribute('sur')) : sur;
                sub = (progenitor.getAttribute('sub')) ? getBoolean(progenitor.getAttribute('sub')) : sub;

                const imageList = getImageList();

                if (trace) console.log(resource(constants.notify, 'ImageListMarkup', imageList));

                return (imageList) ? imageListToArray(imageList) : errorHandler(resource(constants.error, 'NotFoundImageList'));

            } else {

                return errorHandler(resource(constants.error, 'NotFoundProgenitor'));

            }

            function imageListToArray(str)
            {
                return str.replace(/((<([^>]+)>))/gi, '').trim().replace(/\r\n|\r|\n/gi, ';').split(';');
            }

            function getImageList()
            {
                const list = getMarkdownList();

                return (list) ? list : lookAgain();

                function lookAgain()
                {
                    const list = getMarkupList();

                    if (list)
                    {
                        if (trace) console.log(resource(constants.notify, 'ListFallback'));
                        return list;

                    } else {

                        return getImageListRetry();

                    }

                }

                function getMarkdownList()
                {
                    return (progenitor.innerHTML) ? progenitor.innerHTML : null;
                }

                function getMarkupList()
                {
                    const el = document.getElementById(slideview.HTMLImageListElement) ? document.getElementById(slideview.HTMLImageListElement) : document.getElementsByTagName('noscript')[0];
                    return (el) ? el.innerHTML : null;
                }

                function getImageListRetry(retry = 1, retryLimit = 15)
                {
                    if (trace) console.log(resource(constants.notify, 'ListRetryAttempt', retry));

                    try
                    {
                        const list = getMarkdownList() ? getMarkdownList() : getMarkupList();
                        if (!list) throw 'ListNotFoundException';

                    } catch (ex) {

                        if (retry != retryLimit) getImageListRetry(++retry);
                   }

                   return list;
                }

            }

        }

    }

    function getSlideView()
    {
        if (css) importSlideViewStylesheet();

        createSlideViewContainer();

        function createSlideViewContainer()
        {
            progenitor.innerHTML = null;

            const firstChild = document.createElement('div');

            firstChild.id = slideview.HTMLSlideViewElement + '-image-container';
            progenitor.appendChild(firstChild);

            composeAttribute(firstChild.id, 'class', 'slideview-image-container');

            for (let item = 0; item < attributes.length; item++)
            {
                var arrayItem = attributes[item].split(',');

                let qualifier = item + 1;
                let progeny = 'slideview' + qualifier;

                let thirdChildren = {
                    'surName': 'slideview-sur' + qualifier,
                    'imgName': 'slideview-img' + qualifier,
                    'subName': 'slideview-sub' + qualifier
                };

                composeElement('div', secondChild, 'slideview fade', firstChild, null, null, null, null);

                let secondChild = document.getElementById(progeny);

                if (sur) composeElement('div', thirdChildren.surName, 'surtitle', secondChild, getSurtitle(qualifier), null, null, null);
                composeElement('img', thirdChildren.imgName, null, secondChild, null, 'ceres.openImageTab(this);', getURL(), getAccessibilityText())
                if (sub) composeElement('div', thirdChildren.subName, 'subtitle', secondChild, getSubtitle(), null, null, null);
            }

            composeElement('a', 'slideview-prev', 'prev', firstChild, '&#10094;', 'ceres.getSlide(-1, true)', getURL(), null);
            composeElement('a', 'slideview-next', 'next', firstChild, '&#10095;', 'ceres.getSlide(1, true)', getURL(), null);

            if (ptr) createSlideViewPointerContainer();

            setSlideViewDisplay('none');

            function createSlideViewPointerContainer()
            {
                progenitor.appendChild(document.createElement('br'));

                const pointerElement = document.createElement('div');

                pointerElement.id = slideview.HTMLSlideViewElement + '-pointer-container';
                progenitor.appendChild(pointerElement);

                composeAttribute(pointerElement.id, 'class', 'slideview-pointer-container');

                for (let item = 0; item < attributes.length; item++)
                {
                    let qualifier = item + 1;
                    let svpname = 'slideview-ptr' + qualifier;

                    composeElement('span', svpname, 'ptr', pointerElement, null, getClickEventValue(qualifier), null, null);
                }

                progenitor.appendChild(document.createElement('br'));

                if (trace) console.log(resource(constants.notify, 'ProgenitorInnerHTML'));

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
                return (sur) ? indexItem + ' / ' + attributes.length : null;
            }

            function getSubtitle()
            {
                return (sub) ? getAccessibilityText() : null;
            }

            function getAccessibilityText()
            {
                return (arrayItem[1]) ? arrayItem[1].trim() : null;
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
                    if (trace) console.log(resource(constants.notify, 'LinkOnload'));
                }

            }

            function addEventListener()
            {
                if (link.addEventListener)
                {
                    link.addEventListener('load', function()
                    {
                        if (trace) console.log(resource(constants.notify, 'LinkAddEventListener'));
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
                        if (trace) console.log(resource(constants.notify, 'LinkStylesheetCount'));
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
                        if (trace) console.log(resource(constants.notify, 'LinkOnReadyState'));
                    }

                };

            }

        }

    }

    function displaySlide(targetIndex)
    {
        const slides = document.querySelectorAll(".slideview");
        const pointers = document.querySelectorAll(".ptr");

        index = (targetIndex < 1) ? slides.length : (targetIndex > slides.length) ? 1 : index;

        slides.forEach(node => { node.style.display = 'none'; } );
        slides[index-1].style.display = 'block';

        if (ptr)
        {
            pointers.forEach(node => { node.className = node.className.replace(' active', ''); } );
            pointers[index-1].className += ' active';
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

    function loadSlideView()
    {
        setTimeout(initiateSlideView, constants.renderdelay);
    };

    function activateSlideView()
    {
        progenitor.style.display = 'none';

        getSlideView();
        displaySlide();

        setTimeout(function() { setSlideViewDisplay('block'); }, constants.renderdelay / 2);
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

        if (trace) alert(err);

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

    function resource(type, name, str)
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
                'LinkOnload': 'Link default stylesheet insert [' + slideview.HTMLSlideViewElement + ']: onload listener',
                'LinkAddEventListener': 'Link default stylesheet insert [' + slideview.HTMLSlideViewElement + ']: addEventListener',
                'LinkStylesheetCount': 'Link default stylesheet insert [' + slideview.HTMLSlideViewElement + ']: styleSheets.length increment',
                'LinkOnReadyState': 'Link default stylesheet insert [' + slideview.HTMLSlideViewElement + ']: onreadystatechange event',
                'ProgenitorInnerHTML': 'Progenitor innerHTML [' + slideview.HTMLSlideViewElement + ']: ' + newline + progenitor.innerHTML,
                'ImageListMarkup': 'Image list markup [' + slideview.HTMLSlideViewElement + ']: ' + newline + str,
                'ListFallback': 'Image list [' + slideview.HTMLImageListElement + ']: found on the second attempt in the element fallback location',
                'ListRetryAttempt': 'Image list search retry attempt: ' + str,
                'default': 'An unexpected error has occurred - ' + slideview.HTMLSlideViewElement + ' trace notification is unresponsive'
            };

            return lookup[name] || lookup['default'];
        }

        function lookupError()
        {
            const lookup = {
                'NotFoundImageList': 'Error: The ' + slideview.HTMLSlideViewElement + ' document element was found but the ' + slideview.HTMLImageListElement + ' image list could not be read',
                'NotFoundProgenitor': 'Error: Unable to find the ' + slideview.HTMLSlideViewElement + ' document element',
                'default': 'An unexpected error has occurred - ' + slideview.HTMLSlideViewElement + ' error notification is unresponsive'
            };

            return lookup[name] || lookup['default'];
        }

    }

})(ceres);
