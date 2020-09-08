var ceres = {};
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
            this.innerHTML = (src) ? (src.length > 0) ? await (await fetch(src)).text() : null : null;
        }

    });

    let progenitor = null; // parent slideview place holder
    let attributes = null; // slideview element item attributes array
    let trace = false; // default element attribute - enable the trace environment directive
    let ptr = true; // default element attribute - display slideview item pointers
    let sub = true; // default element attribute - display slideview item subtitles
    let sur = true; // default element attribute - display slideview item surtitles
    let css = true; // default element attribute - use the default slideview stylesheet

    let index = 1;

    slideview.openImageTab = function(el) { window.open(el.getAttribute('src'), 'image'); };
    slideview.getSlide = function(targetIndex) { displaySlide(index += targetIndex); };
    slideview.setSlide = function(targetIndex) { displaySlide(index = targetIndex); };

    slideview.slideViewer = function()
    {
        progenitor = (document.getElementById(slideview.container)) ? document.getElementById(slideview.container) : document.getElementsByTagName(slideview.container)[0];
        attributes = getSlideViewAttributes();

        if (attributes) getSlideViewer();

        function getSlideViewAttributes()
        {
            if (progenitor)
            {
                trace = (progenitor.getAttribute('trace')) ? getBoolean(progenitor.getAttribute('trace')) : trace;
                ptr = (progenitor.getAttribute('ptr')) ? getBoolean(progenitor.getAttribute('ptr')) : ptr;
                sub = (progenitor.getAttribute('sub')) ? getBoolean(progenitor.getAttribute('sub')) : sub;
                sur = (progenitor.getAttribute('sur')) ? getBoolean(progenitor.getAttribute('sur')) : sur;
                css = (progenitor.getAttribute('css')) ? getBoolean(progenitor.getAttribute('css')) : css;

                //let imageList = (progenitor.innerHTML) ? progenitor.innerHTML : getEmbedImageList();
                let imageList = awaitImageList();

                if (trace) console.log('Image list: ' + imageList);

                return (imageList) ? imageListToArray(imageList) : errorHandler('ERROR_NotFoundImageList');

            } else {

                return errorHandler('ERROR_NotFoundProgenitor');

            }

            function imageListToArray(str)
            {
                return str.replace(/((<([^>]+)>))/gi, '').trim().replace(/\r\n|\r|\n/gi, ';').split(';');
            }

            function getEmbedImageList()
            {
                return (document.getElementById(slideview.imagelist)) ? document.getElementById(slideview.imagelist).innerHTML : null;
            }

            async function awaitImageList()
            {
                return await getImageList();
            }

            async function getImageList()
            {
                return (progenitor.innerHTML) ? progenitor.innerHTML : getEmbedImageList();
            }

        }

    }

    function displaySlide(targetIndex)
    {
        const slides = document.querySelectorAll(".slideview");
        const pointers = document.querySelectorAll(".ptr");

        index = (targetIndex < 1) ? slides.length : (targetIndex > slides.length) ? 1 : index;

        slides.forEach(slide => { slide.style.display = 'none';	});
        slides[index-1].style.display = 'block';

        if (ptr)
        {
            pointers.forEach(pointer => { pointer.className = pointer.className.replace(' active', '');	});
            pointers[index-1].className += ' active';
        }

    }

    function getSlideViewer()
    {
        if (css) linkSlideViewerListener();

        createSlideViewContainer();
        createSlideviewPointerContainer();

        displaySlide();

        function createAttribute(id, type, value)
        {
            let el = document.getElementById(id);

            if (el)
            {
                let attribute = document.createAttribute(type);
                attribute.value = value;

                el.setAttributeNode(attribute);
            }

        }

        function createSlideViewContainer()
        {
            progenitor.innerHTML = null;

            const descendant = document.createElement('div');
            let progeny = null;

            descendant.id = slideview.container + '-image-container';
            progenitor.appendChild(descendant);

            createAttribute(descendant.id, 'class', 'slideview-image-container');

            for (let item = 0; item < attributes.length; item++)
            {
                var arrayItem = attributes[item].split(',');
                var qualifier = item + 1;

                let svcname = 'slideview' + qualifier;
                let surName = 'slideview-sur' + qualifier;
                let imgName = 'slideview-img' + qualifier;
                let subName = 'slideview-sub' + qualifier;

                setDivElement(svcname, 'slideview fade', descendant, null);

                progeny = document.getElementById(svcname);

                if (sur) setDivElement(surName, 'surtitle', progeny, getSurtitle());
                setImgElement(imgName, 'ceres.openImageTab(this);', progeny);
                if (sub) setDivElement(subName, 'subtitle', progeny, getSubtitle());
            }

            setAElement('slideview-prev', 'prev', 'ceres.getSlide(-1)', descendant, '&#10094;');
            setAElement('slideview-next', 'next', 'ceres.getSlide(1)', descendant, '&#10095;');

            function getURL()
            {
                return (arrayItem[0]) ? arrayItem[0].trim() : null;
            }

            function getSurtitle()
            {
                return (sur) ? qualifier + ' / ' + attributes.length : null;
            }

            function getSubtitle()
            {
                return (sub) ? (arrayItem[1]) ? arrayItem[1].trim() : null : null;
            }

            function setDivElement(id, classValue, parent, markup)
            {
                let el = document.createElement('div');

                el.id = id;
                parent.appendChild(el);

                createAttribute(el.id, 'class', classValue);

                if (markup) document.getElementById(el.id).innerHTML = markup;
            }

            function setImgElement(id, onClickEventValue, parent)
            {
                let el = document.createElement('img');

                el.id = id;
                parent.appendChild(el);

                createAttribute(el.id, 'onclick', onClickEventValue);
                createAttribute(el.id, 'src', getURL());
            }

            function setAElement(id, classValue, onClickEventValue, parent, markup)
            {
                let el = document.createElement('a');

                el.id = id;
                parent.appendChild(el);

                createAttribute(el.id, 'class', classValue);
                createAttribute(el.id, 'onclick', onClickEventValue);
                createAttribute(el.id, 'src', getURL());

                if (markup) document.getElementById(el.id).innerHTML = markup;
            }

        }

        function createSlideviewPointerContainer()
        {
            if (!ptr) return;

            progenitor.appendChild(document.createElement('br'));

            const descendant = document.createElement('div');

            descendant.id = slideview.container + '-pointer-container';
            progenitor.appendChild(descendant);

            createAttribute(descendant.id, 'class', 'slideview-pointer-container');

            for (let item = 0; item < attributes.length; item++)
            {
                var qualifier = item + 1;
                let svpname = 'slideview-ptr' + qualifier;

                setSpanElement(svpname, 'ptr', getClickEventValue(), descendant);
            }

            progenitor.appendChild(document.createElement('br'));

            if (trace) console.log('Progenitor: ' + progenitor.innerHTML);

            function getClickEventValue()
            {
                return 'ceres.setSlide(' + qualifier + ')';
            }

            function setSpanElement(id, classValue, onClickEventValue, parent)
            {
                let el = document.createElement('span');

                el.id = id;
                parent.appendChild(el);

                createAttribute(el.id, 'class', classValue);
                createAttribute(el.id, 'onclick', onClickEventValue);
            }

        }

        function linkSlideViewerListener()
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
                    if (trace) console.log(ceresResource('NOTIFY_LinkOnload'));
                }

            }

            function addEventListener()
            {
                if (link.addEventListener)
                {
                    link.addEventListener('load', function()
                    {
                        if (trace) console.log(ceresResource('NOTIFY_LinkAddEventListener'));
                    }, false);

                }

            }

            function styleSheetsLengthListener()
            {
                var cssnum = document.styleSheets.length;

                var ti = setInterval(function()
                {
                    if (document.styleSheets.length > cssnum)
                    {
                        clearInterval(ti);
                        if (trace) console.log(ceresResource('NOTIFY_LinkStylesheetCount'));
                    }

                }, 10);

            }

            function onReadyStateChangeListener()
            {
                link.onreadystatechange = function()
                {
                    var state = link.readyState;

                    if (state === 'loaded' || state === 'complete')
                    {
                        link.onreadystatechange = null;
                        if (trace) console.log(ceresResource('NOTIFY_LinkOnReadyState'));
                    }

                };

            }

        }

    }

    function getBoolean(value)
    {
        let symbol = value.trim().toUpperCase();

        if (!symbol) return false;

        switch (symbol)
        {
            case 'TRUE': return true;
            case 'T': return true;
            case 'YES': return true;
            case 'Y': return true;
            case '1': return true;
            default: return false;
        }

    }


    function errorHandler(name)
    {
        let err = 'ERROR: ' + ceresResource(name) + '. DateTime: ' + new Date().toLocaleString();

        console.log(err);
        alert(err);

        return null;
    }

    function ceresResource(name)
    {
        switch (name)
        {
          case 'ERROR_NotFoundImageList': return 'The ' + slideview.container + ' document element was found but the ' + slideview.imagelist + ' image list could not be read';
          case 'ERROR_NotFoundProgenitor': return 'Unable to find the ' + slideview.container + ' document element';
          case 'NOTIFY_LinkOnload': return 'Link insert: onload listener';
          case 'NOTIFY_LinkAddEventListener': return 'Link insert: addEventListener';
          case 'NOTIFY_LinkStylesheetCount': return 'Link insert: styleSheets.length increment';
          case 'NOTIFY_LinkOnReadyState': return 'Link insert: onreadystatechange event';
          default: return 'An unexpected error has occurred. The slide viewer has stopped';
        }

    }

})(ceres);
