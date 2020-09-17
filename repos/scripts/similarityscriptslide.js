let ceres = {};
(function(slideview)
{
    'use strict';

    slideview.HTMLSlideViewElement = 'ceres-slideview'; // required public element name and id
    slideview.HTMLImageListElement = 'ceres-csv'; // optional public markup noscript tag id when using embedded image lists
    slideview.defaultCSS = 'https://ceresbakalite.github.io/similarity/stylesheets/similaritysheetslide.css'; // public attribute pointing to the default slideview stylesheet
    slideview.tabImage = function(el) { window.open(el.getAttribute('src'), 'image'); }; // public method reference
    slideview.getSlide = function(target, calc) { getSlide(csv.index = (calc) ? csv.index += target : target); };  // public method reference

    window.customElements.define(slideview.HTMLSlideViewElement, class extends HTMLElement
    {
        async connectedCallback()
        {
            let css = (this.getAttribute('css')) ? getBoolean(this.getAttribute('css')) : true;
            if (css) await ( await importSlideViewStylesheet() );

            let src = this.getAttribute('src');
            if (src) this.innerHTML =  await ( await fetch(src)).text();

            initiateSlideView();
        }

    })

    class components
    {
        constructor()
        {
            this.types = function() { return type; },
            this.attributes = function() { return attribute; }
        }

    }

    let resource = new components();

    class slideviewer
    {
        constructor()
        {
            this.progenitor = null;
            this.imageArray = null,
            this.imageContainer = null,
            this.slideContainer = null,
            this.attributes = function() { return attribute; },
            this.index = 1
        }

    }

    let csv = new slideviewer();

    function initiateSlideView()
    {
        csv.progenitor = (document.getElementById(slideview.HTMLSlideViewElement)) ? document.getElementById(slideview.HTMLSlideViewElement) : document.getElementsByTagName(slideview.HTMLSlideViewElement)[0];
        csv.imageArray = getImageArray();

        if (csv.imageArray) activateSlideView();

        function getImageArray()
        {
            if (!csv.progenitor) return response(resource.types.error, resource.attributes.ProgenitorNotFound);

            let imageList = getImageList();

            response(resource.types.notify, resource.attributes.ListContainerMarkup + imageList);

            return (imageList) ? imageList.trim().replace(/\r\n|\r|\n/gi, ';').split(';') : null;

            function getImageList()
            {
                if (!getSlideviewAttributes()) return response(resource.types.error, resource.attributes.ListContainerNotFound);

                return (resource.attributes.ProgenitorSource) ? getConnectedCallbackList() : getBodyContentList();

                function getConnectedCallbackList()
                {
                    return (csv.progenitor.textContent) ? csv.progenitor.textContent : null;
                }

                function getBodyContentList()
                {
                    response(resource.types.notify, resource.attributes.BodyContentList);

                    const el = document.getElementById(slideview.HTMLImageListElement) ? document.getElementById(slideview.HTMLImageListElement) : document.getElementsByTagName('noscript')[0];
                    const list = (el) ? el.textContent : null;

                    return (list) ? list : response(resource.types.error, resource.attributes.BodyContentListNotFound);
                }

            }

            function getSlideviewAttributes()
            {
                csv.progenitor.id = slideview.HTMLSlideViewElement;

                const newline = '\n';

                resource.types.reference = 1;
                resource.types.notify = 2;
                resource.types.error = 99;
                resource.attributes.ProgenitorSource = csv.progenitor.getAttribute('src') ? true : false;
                resource.attributes.ProgenitorInnerHTML = 'Progenitor innerHTML [' + slideview.HTMLSlideViewElement + ']: ' + newline;
                resource.attributes.ProgenitorNotFound = 'Error: Unable to find the ' + slideview.HTMLSlideViewElement + ' document element';
                resource.attributes.ListContainerMarkup = 'Image list markup ' + ((resource.attributes.ProgenitorSource) ? 'sourced from connectedCallback' : 'sourced from document body') + ' [' + slideview.HTMLSlideViewElement + ']:' + newline;
                resource.attributes.ListContainerNotFound = 'Error: Unable to find either the connectedCallback ' + slideview.HTMLSlideViewElement + ' attribute source nor the fallback noscript image list container';
                resource.attributes.BodyContentList = 'The ' + slideview.HTMLSlideViewElement + ' src attribute url is unavailable. Searching for the fallback noscript image list content in the document body';
                resource.attributes.BodyContentListNotFound = 'Error: Unable to find the ' + slideview.HTMLSlideViewElement + ' fallback noscript image list when searching the document body';
                resource.attributes.LinkOnload = 'Link default stylesheet insert [' + slideview.HTMLSlideViewElement + ']: onload listener';
                resource.attributes.LinkAddEventListener = 'Link default stylesheet insert [' + slideview.HTMLSlideViewElement + ']: addEventListener';
                resource.attributes.LinkOnReadyState = 'Link default stylesheet insert [' + slideview.HTMLSlideViewElement + ']: onreadystatechange event';
                resource.attributes.CSVObjectAttributes = 'The csv object attributes properties after initialisation [' + slideview.HTMLSlideViewElement + ']: ';

                Object.freeze(resource.attributes);

                csv.attributes.trace = (csv.progenitor.getAttribute('trace')) ? getBoolean(csv.progenitor.getAttribute('trace')) : false;
                csv.attributes.css = (csv.progenitor.getAttribute('css')) ? getBoolean(csv.progenitor.getAttribute('css')) : true;
                csv.attributes.ptr = (csv.progenitor.getAttribute('ptr')) ? getBoolean(csv.progenitor.getAttribute('ptr')) : true;
                csv.attributes.sur = (csv.progenitor.getAttribute('sur')) ? getBoolean(csv.progenitor.getAttribute('sur')) : true;
                csv.attributes.sub = (csv.progenitor.getAttribute('sub')) ? getBoolean(csv.progenitor.getAttribute('sub')) : true;

                Object.freeze(csv.attributes);

                response(resource.types.notify, resource.attributes.CSVObjectAttributes + getAttributeProperties());

                return getListContainerConfirmation();

                function getAttributeProperties()
                {
                    let str = '';
                    for (let property in csv.attributes) str += property + ": " + csv.attributes[property] + ', ';
                    return str.replace(/, +$/g,'');
                }

                function getListContainerConfirmation()
                {
                    const el = document.getElementById(slideview.HTMLImageListElement) ? document.getElementById(slideview.HTMLImageListElement) : document.getElementsByTagName('noscript')[0];
                    return (csv.progenitor.getAttribute('src') || el) ? true : false;
                }

            }

        }

    }

    function response(type, attribute)
    {
        const lookup = {
            [resource.types.notify]: function() { if (csv.attributes.trace) console.log(attribute); },
            [resource.types.error]: function() { return errorHandler(attribute); },
            'default': 'An unexpected error has occurred - ' + slideview.HTMLSlideViewElement + ' response notification is unresponsive'
        };

        return lookup[type]() || lookup['default'];
    }

    function getSlideView()
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
            composeElement('img', elements.imgName, 'slide', csv.slideContainer, null, 'ceres.tabImage(this);', getURL(), getAccessibilityText())
            if (csv.attributes.sub) composeElement('div', elements.subName, 'subtitle', csv.slideContainer, getSubtitle(), null, null, null);
        }

        composeElement('a', 'slideview-prev', 'prev', csv.imageContainer, '&#10094;', 'ceres.getSlide(-1, true)', getURL(), null);
        composeElement('a', 'slideview-next', 'next', csv.imageContainer, '&#10095;', 'ceres.getSlide(1, true)', getURL(), null);

        if (csv.attributes.ptr) getSlideViewPointerContainer();

        setSlideViewDisplay('none');

        response(resource.types.notify, resource.attributes.ProgenitorInnerHTML + csv.progenitor.innerHTML);

        function getSlideViewPointerContainer()
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

    function importSlideViewStylesheet()
    {
        const link = document.createElement('link');

        link.rel = 'stylesheet';
        link.type = 'text/css';
        link.href = slideview.defaultCSS;
        link.as = 'style';

        onloadListener();
        addEventListener();
        onReadyStateChangeListener();

        document.head.appendChild(link);

        function onloadListener()
        {
            link.onload = function ()
            {
                response(resource.types.notify, resource.attributes.LinkOnload);
            }

        }

        function addEventListener()
        {
            if (link.addEventListener)
            {
                link.addEventListener('load', function()
                {
                    response(resource.types.notify, resource.attributes.LinkAddEventListener);
                }, false);

            }

        }

        function onReadyStateChangeListener()
        {
            link.onreadystatechange = function()
            {
                const state = link.readyState;

                if (state === 'loaded' || state === 'complete')
                {
                    link.onreadystatechange = null;
                    response(resource.types.notify, resource.attributes.LinkOnReadyState);
                }

            };

        }

    }

    function getSlide(targetIndex)
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
        const renderdelay = 500; // awaiting slideview css catchup

        csv.progenitor.style.display = 'none';

        getSlideView();
        getSlide();

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

})(ceres);
