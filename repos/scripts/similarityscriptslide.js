let ceres = {};
(function(slideview)
{
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

    const renderdelay = 500; // onload setTimeout period in ms
    const notify = 1; // console notification type
    const error = 99; // console notification type

    const testresource =
    {
        testnotify: 1,
        testerror: 99
    };

    let progenitor = null; // parent slideview place holder
    let attributes = null; // slideview element item attributes array
    let trace = false; // default element attribute - enable slideview trace environment directive
    let ptr = true; // default element attribute - display slideview item pointers
    let css = true; // default element attribute - use the default slideview stylesheet
    let sur = true; // default element attribute - display slideview item surtitles
    let sub = true; // default element attribute - display slideview item subtitles
    let index = 1; // pointer referencing to the currently active slide

    alert('testnotifyLookup: ' + test(testresource.testnotify));
    alert('testerrorLookup: ' + test(testresource.testerror));


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
                const list = getMarkdownList();

                return (list) ? list : lookAgain();

                function lookAgain()
                {
                    const list = getMarkupList();

                    if (list)
                    {
                        if (trace) console.log(resource(notify, 'ListFallback'));
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
                    if (trace) console.log(resource(notify, 'ListRetryAttempt', retry));

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

            const imageElement = document.createElement('div');

            imageElement.id = slideview.HTMLSlideViewElement + '-image-container';
            progenitor.appendChild(imageElement);

            composeAttribute(imageElement.id, 'class', 'slideview-image-container');

            for (let item = 0; item < attributes.length; item++)
            {
                var arrayItem = attributes[item].split(',');

                let qualifier = item + 1;
                let svcName = 'slideview' + qualifier;
                let surName = 'slideview-sur' + qualifier;
                let imgName = 'slideview-img' + qualifier;
                let subName = 'slideview-sub' + qualifier;

                composeElement('div', svcName, 'slideview fade', imageElement, null, null, null, null);

                let progeny = document.getElementById(svcName);

                if (sur) composeElement('div', surName, 'surtitle', progeny, getSurtitle(qualifier), null, null, null);
                composeElement('img', imgName, null, progeny, null, 'ceres.openImageTab(this);', getURL(), getAccessibilityText())
                if (sub) composeElement('div', subName, 'subtitle', progeny, getSubtitle(), null, null, null);
            }

            composeElement('a', 'slideview-prev', 'prev', imageElement, '&#10094;', 'ceres.getSlide(-1, true)', getURL(), null);
            composeElement('a', 'slideview-next', 'next', imageElement, '&#10095;', 'ceres.getSlide(1, true)', getURL(), null);

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
                return (sub) ? getAccessibilityText() : null;
            }

            function getAccessibilityText()
            {
                return (arrayItem[1]) ? arrayItem[1].trim() : null;
            }

        }

        function importSlideViewStylesheet()
        {
            const defaultCSS = 'https://ceresbakalite.github.io/similarity/stylesheets/similaritysheetslide.css';
            const link = document.createElement('link');

            link.rel = 'stylesheet';
            link.type = 'text/css';
            link.href = defaultCSS;
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

                const ti = setInterval(function()
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
        setTimeout(initiateSlideView, renderdelay);
    };

    function activateSlideView()
    {
        progenitor.style.display = 'none';

        getSlideView();
        displaySlide();

        setTimeout(function() { setSlideViewDisplay('block'); }, renderdelay / 2);
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


    function test(type)
    {
        alert(type);

        const lookup = {
            [testresource.testnotify]: 'testnotify found',
            [testresource.testerror]: 'testerror found'
        };

        return lookup[type] || 'lost';

    }

    function resource(type, name, str)
    {
        const newline = '\n';

        switch (type)
        {
            case notify: return lookupNotify();
            case error: return lookupError();
            default: return 'An unexpected error has occurred - ' + slideview.HTMLSlideViewElement + ' is unresponsive';
        }

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
