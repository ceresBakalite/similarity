export { ceres }

var ceres = {};
(function(slideview)
{
    'use strict';

    window.tabImage = function(el) { window.open(el.getAttribute('src'), 'image'); }; // public method reference
    window.getSlide = function(target, calc) { getSlide(csv.index = (calc) ? csv.index += target : target); };  // public method reference

    slideview.HTMLSlideViewElement = 'ceres-slideview'; // required (light) public element name and id
    slideview.HTMLImageListElement = 'ceres-csv'; // optional (light) public markup noscript tag id when using embedded image lists
    slideview.defaultCSS = 'https://ceresbakalite.github.io/similarity/repos/modules/ceres-sv.css'; // public attribute pointing to the default slideview stylesheet

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

    class Component
    {
        constructor()
        {
            this.type = function() { return type; },
            this.attribute = function() { return attribute; }
        }

    }

    let resource = new Component();

    class Slideviewer
    {
        constructor()
        {
            this.progenitor = null;
            this.imageArray = null,
            this.imageContainer = null,
            this.slideContainer = null,
            this.listElement = null,
            this.attribute = function() { return attribute; },
            this.callback = false,
            this.activate = false;
            this.index = 1
        }

    }

    let csv = new Slideviewer();

    function initiateSlideView()
    {
        csv.activate = getSlideviewAttributes();
        if (csv.activate) activateSlideView();
    }

    function getSlideviewAttributes()
    {
        const newline = '\n';

        if (!getProgenitor()) return inspect(resource.type.error, resource.attribute.ProgenitorNotFound);
        if (!getAttributePrecursors()) return inspect(resource.type.error, resource.attribute.ListContainerNotFound);

        resource.attribute.ProgenitorInnerHTML = 'Progenitor innerHTML [' + slideview.HTMLSlideViewElement + ']: ' + newline + newline;
        resource.attribute.ListContainerMarkup = 'Image list markup ' + ((csv.callback) ? 'delivered as promised by connectedCallback' : 'sourced from the document body') + ' [' + slideview.HTMLSlideViewElement + ']:' + newline;
        resource.attribute.BodyContentList = 'The ' + slideview.HTMLSlideViewElement + ' src attribute url is unavailable. Searching for the fallback noscript image list content in the document body';
        resource.attribute.BodyContentListNotFound = 'Error: Unable to find the ' + slideview.HTMLSlideViewElement + ' fallback noscript image list when searching the document body';
        resource.attribute.CSVObjectAttributes = 'The csv object attribute properties after initialisation [' + slideview.HTMLSlideViewElement + ']: ';

        Object.freeze(resource.attribute);

        return getImageArray();

        function getProgenitor()
        {
            resource.type.reference = 1;
            resource.type.notify = 2;
            resource.type.error = 99;

            resource.attribute.ProgenitorNotFound = 'Error: Unable to find the ' + slideview.HTMLSlideViewElement + ' document element';
            resource.attribute.ListContainerNotFound = 'Error: Unable to find either the connectedCallback ' + slideview.HTMLSlideViewElement + ' attribute source nor the fallback noscript image list container';

            csv.progenitor = (document.getElementById(slideview.HTMLSlideViewElement)) ? document.getElementById(slideview.HTMLSlideViewElement) : document.getElementsByTagName(slideview.HTMLSlideViewElement)[0];

            return (csv.progenitor) ? true : false;
        }

        function getAttributePrecursors()
        {
            csv.progenitor.id = slideview.HTMLSlideViewElement;

            csv.listElement = document.getElementById(slideview.HTMLImageListElement) ? document.getElementById(slideview.HTMLImageListElement) : document.getElementsByTagName('noscript')[0];
            csv.callback = csv.progenitor.getAttribute('src') ? true : false;

            csv.attribute.trace = (csv.progenitor.getAttribute('trace')) ? getBoolean(csv.progenitor.getAttribute('trace')) : false;
            csv.attribute.css = (csv.progenitor.getAttribute('css')) ? getBoolean(csv.progenitor.getAttribute('css')) : true;
            csv.attribute.ptr = (csv.progenitor.getAttribute('ptr')) ? getBoolean(csv.progenitor.getAttribute('ptr')) : true;
            csv.attribute.sur = (csv.progenitor.getAttribute('sur')) ? getBoolean(csv.progenitor.getAttribute('sur')) : true;
            csv.attribute.sub = (csv.progenitor.getAttribute('sub')) ? getBoolean(csv.progenitor.getAttribute('sub')) : true;

            Object.freeze(csv.attribute);

            return (csv.callback || csv.listElement) ? true : false;
        }

        function getImageArray()
        {
            inspect(resource.type.notify, resource.attribute.CSVObjectAttributes + getAttributeProperties());

            let imageList = getImageList();
            if (imageList) inspect(resource.type.notify, resource.attribute.ListContainerMarkup + imageList);

            csv.imageArray = (imageList) ? imageList.trim().replace(/\r\n|\r|\n/gi, ';').split(';') : null;

            return (csv.imageArray) ? true : false;

            function getImageList()
            {
                return (csv.callback) ? getConnectedCallbackList() : getBodyContentList();

                function getConnectedCallbackList()
                {
                    return (csv.progenitor.textContent) ? csv.progenitor.textContent : null;
                }

                function getBodyContentList()
                {
                    inspect(resource.type.notify, resource.attribute.BodyContentList);

                    const list = (csv.listElement) ? csv.listElement.textContent : null;
                    return (list) ? list : inspect(resource.type.error, resource.attribute.BodyContentListNotFound);
                }

            }

            function getAttributeProperties()
            {
                let str = '';
                for (let property in csv.attribute) str += property + ": " + csv.attribute[property] + ', ';

                return str.replace(/, +$/g,'');
            }

        }

    }

    function inspect(type, response)
    {
        const lookup = {
            [resource.type.notify]: function() { if (csv.attribute.trace) console.log(response); },
            [resource.type.error]: function() { errorHandler(response); },
            'default': 'An unexpected error has occurred - ' + slideview.HTMLSlideViewElement + ' is unresponsive'
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

            if (csv.attribute.sur) composeElement('div', elements.surName, 'surtitle', csv.slideContainer, getSurtitle(qualifier), null, null, null);
            composeElement('img', elements.imgName, 'slide', csv.slideContainer, null, 'window.tabImage(this);', getURL(), getAccessibilityText())
            if (csv.attribute.sub) composeElement('div', elements.subName, 'subtitle', csv.slideContainer, getSubtitle(), null, null, null);
        }

        composeElement('a', 'slideview-prev', 'prev', csv.imageContainer, '&#10094;', 'window.getSlide(-1, true)', getURL(), null);
        composeElement('a', 'slideview-next', 'next', csv.imageContainer, '&#10095;', 'window.getSlide(1, true)', getURL(), null);

        if (csv.attribute.ptr) getSlideViewPointerContainer();

        setSlideViewDisplay('none');

        inspect(resource.type.notify, resource.attribute.ProgenitorInnerHTML + csv.progenitor.innerHTML);

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
                return 'window.getSlide(' + indexItem + ')';
            }

        }

        function getURL()
        {
            return (arrayItem[0]) ? arrayItem[0].trim() : null;
        }

        function getSurtitle(indexItem)
        {
            return (csv.attribute.sur) ? indexItem + ' / ' + csv.imageArray.length : null;
        }

        function getSubtitle()
        {
            return (csv.attribute.sub) ? getAccessibilityText() : null;
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
            link.onload = function () {}
        }

        function addEventListener()
        {
            if (link.addEventListener)
            {
                link.addEventListener('load', function() {}, false);
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

        if (csv.attribute.ptr)
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

        if (csv.attribute.trace) alert(err);

        return false;
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
