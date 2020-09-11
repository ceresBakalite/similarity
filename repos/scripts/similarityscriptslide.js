let ceres = {};
(function(slideview)
{
    slideview.defaultStylesheet = 'https://ceresbakalite.github.io/similarity/stylesheets/similaritysheetslide.css';
    slideview.container = 'ceres-slideview';
    slideview.imagelist = 'ceres-csv';

    window.customElements.define(slideview.container, class extends HTMLElement
    {
        async connectedCallback()
        {
            let src = this.getAttribute('src');
            this.innerHTML = await (await fetch(src)).text();
        }

    });

    let progenitor = null; // parent slideview place holder
    let attributes = null; // slideview element item attributes array
    let trace = false; // default element attribute - enable slideview trace environment directive
    let ptr = true; // default element attribute - display slideview item pointers
    let sub = true; // default element attribute - display slideview item subtitles
    let sur = true; // default element attribute - display slideview item surtitles
    let css = true; // default element attribute - use the default slideview stylesheet

    let index = 1;

    slideview.openImageTab = function(el) { window.open(el.getAttribute('src'), 'image'); };
    slideview.getSlide = function(target, calc) { displaySlide(index = (calc) ? index += target : target); };
    slideview.slideViewer = function() { setTimeout(initiateSlideViewer, 500); };

    const notify = 1;
    const error = 99;

    function initiateSlideViewer()
    {
        progenitor = (document.getElementById(slideview.container)) ? document.getElementById(slideview.container) : document.getElementsByTagName(slideview.container)[0];
        attributes = getSlideViewerAttributes();

        if (attributes) activateSlideViewer();

        function getSlideViewerAttributes()
        {
            if (progenitor)
            {
                trace = (progenitor.getAttribute('trace')) ? getBoolean(progenitor.getAttribute('trace')) : trace;
                ptr = (progenitor.getAttribute('ptr')) ? getBoolean(progenitor.getAttribute('ptr')) : ptr;
                sub = (progenitor.getAttribute('sub')) ? getBoolean(progenitor.getAttribute('sub')) : sub;
                sur = (progenitor.getAttribute('sur')) ? getBoolean(progenitor.getAttribute('sur')) : sur;
                css = (progenitor.getAttribute('css')) ? getBoolean(progenitor.getAttribute('css')) : css;

                const imageList = getImageList();

                if (trace) console.log(resource(notify, 'ImageListMarkup', imageList));

                return (imageList) ? imageListToArray(imageList) : errorHandler(resource(error, 'NotFoundImageList'));

            } else {

                return errorHandler(resource(error, 'NotFoundProgenitor'));

            }

            function imageListToArray(str)
            {
                return str.replace(/((<([^>]+)>))/gi, '').trim().replace(/\r\n|\r|\n/gi, ';').split(';');
            }

            function getImageList()
            {
                let list = getMarkdownImageList();

                return (list) ? list : getMarkupImageList();

                function getMarkdownImageList()
                {
                    return (progenitor.innerHTML) ? progenitor.innerHTML : null;
                }

                function getMarkupImageList()
                {
                    let list = (document.getElementById(slideview.imagelist)) ? document.getElementById(slideview.imagelist).innerHTML : null;
                    if (list && trace) console.log(resource('notify', 'ListRetryAttempt'));
                    return list;
                }

            }

        }

    }

    function activateSlideViewer()
    {
        progenitor.style.display = 'none';

        getSlideViewer();
        displaySlide();

        setTimeout(function() { setSlideViewerDisplay('block'); }, 250);
    }

    function displaySlide(targetIndex)
    {
        const slides = document.querySelectorAll(".slideview");
        const pointers = document.querySelectorAll(".ptr");

        index = (targetIndex < 1) ? slides.length : (targetIndex > slides.length) ? 1 : index;

        slides.forEach(node => { node.style.display = 'none';	} );
        slides[index-1].style.display = 'block';

        if (ptr)
        {
            pointers.forEach(node => { node.className = node.className.replace(' active', '');	} );
            pointers[index-1].className += ' active';
        }

    }

    function composeElement(element, id, classValue, parent, markup, onClickEventValue, url)
    {
        const el = document.createElement(element);

        el.id = id;
        parent.appendChild(el);

        if (classValue) composeAttribute(el.id, 'class', classValue);
        if (onClickEventValue) composeAttribute(el.id, 'onclick', onClickEventValue);
        if (url) composeAttribute(el.id, 'src', url);

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

    function setSlideViewerDisplay(attribute)
    {
        const nodelist = document.querySelectorAll('a.prev, a.next, div.subtitle, div.surtitle, #' + slideview.container);
        nodelist.forEach(node => { node.style.display = attribute; } );
    }

    function errorHandler(str)
    {
        const err = str + '. DateTime: ' + new Date().toLocaleString();

        console.log(err);
        alert(err);

        return null;
    }

    function getBoolean(symbol)
    {
        let token = symbol.trim().toUpperCase();

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
        const iNotify = 1;
        const iError = 99;

        let lookup = {

            iNotify: function () {

                const lookupNotify = {
                    'LinkOnload': 'Link default stylesheet insert [' + slideview.container + ']: onload listener',
                    'LinkAddEventListener': 'Link default stylesheet insert [' + slideview.container + ']: addEventListener',
                    'LinkStylesheetCount': 'Link default stylesheet insert [' + slideview.container + ']: styleSheets.length increment',
                    'LinkOnReadyState': 'Link default stylesheet insert [' + slideview.container + ']: onreadystatechange event',
                    'ProgenitorInnerHTML': 'Progenitor innerHTML [' + slideview.container + ']: ' + newline + progenitor.innerHTML,
                    'ImageListMarkup': 'Image list markup [' + slideview.container + ']: ' + newline + str,
                    'ListRetryAttempt': 'Image list [' + slideview.imagelist + ']: found on the second attempt in the element fallback location'
                };

                return lookupNotify[name] || 'An unexpected error has occurred - ' + slideview.container + ' trace notification is unresponsive';
            },

            iError: function () {

                const lookupError = {
                    'NotFoundImageList': 'Error: The ' + slideview.container + ' document element was found but the ' + slideview.imagelist + ' image list could not be read',
                    'NotFoundProgenitor': 'Error: Unable to find the ' + slideview.container + ' document element'
                };

                return lookupError[name] || 'An unexpected error has occurred - ' + slideview.container + ' error notification is unresponsive';
            }

        };

        return lookup[type]();
    }

    function getSlideViewer()
    {
        if (css) importSlideViewerStylesheet();

        createSlideViewerContainer();

        function createSlideViewerContainer()
        {
            progenitor.innerHTML = null;

            const imageElement = document.createElement('div');

            imageElement.id = slideview.container + '-image-container';
            progenitor.appendChild(imageElement);

            composeAttribute(imageElement.id, 'class', 'slideview-image-container');

            for (let item = 0; item < attributes.length; item++)
            {
                var arrayItem = attributes[item].split(',');
                let qualifier = item + 1;

                let svcname = 'slideview' + qualifier;
                let surName = 'slideview-sur' + qualifier;
                let imgName = 'slideview-img' + qualifier;
                let subName = 'slideview-sub' + qualifier;

                composeElement('div', svcname, 'slideview fade', imageElement, null, null, null);

                let progeny = document.getElementById(svcname);

                if (sur) composeElement('div', surName, 'surtitle', progeny, getSurtitle(qualifier), null, null);
                composeElement('img', imgName, null, progeny, 'ceres.openImageTab(this);', null, getURL())
                if (sub) composeElement('div', subName, 'subtitle', progeny, getSubtitle(), null, null);
            }

            createSlideViewerPointerContainer();

            composeElement('a', 'slideview-prev', 'prev', imageElement, '&#10094;', 'ceres.getSlide(-1, true)', getURL());
            composeElement('a', 'slideview-next', 'next', imageElement, '&#10095;', 'ceres.getSlide(1, true)', getURL());

            setSlideViewerDisplay('none');

            function createSlideViewerPointerContainer()
            {
                if (!ptr) return;

                progenitor.appendChild(document.createElement('br'));

                const pointerElement = document.createElement('div');

                pointerElement.id = slideview.container + '-pointer-container';
                progenitor.appendChild(pointerElement);

                composeAttribute(pointerElement.id, 'class', 'slideview-pointer-container');

                for (let item = 0; item < attributes.length; item++)
                {
                    let qualifier = item + 1;
                    let svpname = 'slideview-ptr' + qualifier;

                    composeElement('span', svpname, 'ptr', pointerElement, null, getClickEventValue(qualifier), null);
                }

                progenitor.appendChild(document.createElement('br'));

                if (trace) console.log(resource(notify, 'ProgenitorInnerHTML'));

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
                return (sub) ? (arrayItem[1]) ? arrayItem[1].trim() : null : null;
            }

        }

        function importSlideViewerStylesheet()
        {
            if (!css) return;

            const link = document.createElement('link');

            link.rel = 'stylesheet';
            link.type = 'text/css';
            link.href = slideview.defaultStylesheet;
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
                    if (trace) console.log(resource(notify, 'LinkOnload'));
                }

            }

            function addEventListener()
            {
                if (link.addEventListener)
                {
                    link.addEventListener('load', function()
                    {
                        if (trace) console.log(resource(notify, 'LinkAddEventListener'));
                    }, false);

                }

            }

            function styleSheetsLengthListener()
            {
                const cssnum = document.styleSheets.length;

                var ti = setInterval(function()
                {
                    if (document.styleSheets.length > cssnum)
                    {
                        clearInterval(ti);
                        if (trace) console.log(resource(notify, 'LinkStylesheetCount'));
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
                        if (trace) console.log(resource(notify, 'LinkOnReadyState'));
                    }

                };

            }

        }

    }

})(ceres);
