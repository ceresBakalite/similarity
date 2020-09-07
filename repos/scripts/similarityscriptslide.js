var ceres = {};
(function(slideview)
{
    const trace = true; // environment directive
    const progenitor = null; // parent slideviewer place holder
    const attributes = null; // slideviewer element attributes

    let ptr = true; // default - use slideviewer css stylesheet
    let sub = true; // default - display slideviewer pointers
    let sur = true; // default - display slideviewer surtitles
    let css = true; // default - display slideviewer subtitles

    let index = 1;

    window.customElements.define('ceres-slideview', class extends HTMLElement
    {
        async connectedCallback()
        {
            let src = this.getAttribute('src');
            this.innerHTML = await (await fetch(src)).text();
        }

    });

    slideview.openImageTab = function(el) { window.open(el.getAttribute('src'), 'image'); };

    slideview.getSlide = function(targetIndex) { displaySlide(index += targetIndex); };

    slideview.setSlide = function(targetIndex) { displaySlide(index = targetIndex); };

    slideview.slideViewer = function()
    {
        progenitor = (document.getElementById("ceres-slideview")) ? document.getElementById("ceres-slideview") : document.getElementsByTagName('ceres-slideview')[0];
        attributes = getSlideViewerAttributes();

        if (css)
        {
            getSlideViewerScripts();

        } else {
            intialiseSlideViewer();
        }

        function getSlideViewerAttributes()
        {
            let ptr = (progenitor.getAttribute('ptr')) ? progenitor.getAttribute('ptr') : ptr;
            let sub = (progenitor.getAttribute('sub')) ? progenitor.getAttribute('sub') : sub;
            let sur = (progenitor.getAttribute('sur')) ? progenitor.getAttribute('sur') : sur;
            let css = (progenitor.getAttribute('css')) ? progenitor.getAttribute('css') : css;

            return imageListToArray(progenitor.innerHTML);

            function imageListToArray(str)
            {
                return str.replace(/((<([^>]+)>)| )/gi, '').trim().replace(/\r\n|\r|\n/gi, ';').split(';');
            }

        }

    }

    function intialiseSlideViewer()
    {
        getSlideViewer();
        displaySlide();
    }

    function getSlideViewerScripts()
    {
        let aquire = (css) ? true : false;

        const link = document.createElement('link');

        link.rel = 'stylesheet';
        link.type = 'text/css';
        link.href = 'https://ceresbakalite.github.io/similarity/stylesheets/similaritysheetslide.css';
        link.as = 'style';

        if (aquire)
        {
            link.onload = function ()
            {
                invokeSlideViewer('onload listener');
            }

        }

        if (aquire)
        {
            if (link.addEventListener)
            {
                link.addEventListener('load', function()
                {
                    invokeSlideViewer("DOM's load event");
                }, false);

            }

        }

        if (aquire)
        {

            link.onreadystatechange = function()
            {
                var state = link.readyState;

                if (state === 'loaded' || state === 'complete')
                {
                    link.onreadystatechange = null;
                    initialise("onreadystatechange");
                }

            };

        }

        if (aquire)
        {
            var cssnum = document.styleSheets.length;

            var ti = setInterval(function()
            {
                if (document.styleSheets.length > cssnum)
                {
                    clearInterval(ti);
                    invokeSlideViewer('listening to styleSheets.length change');
                }

            }, 10);

        }

        document.head.appendChild(link);

        function invokeSlideViewer(str)
        {
            if (trace) console.log(str);
            aquire = false;

            intialiseSlideViewer();
        }
    }


    function displaySlide(targetIndex)
    {
        const slides = document.querySelectorAll(".slideview");
        const pointers = document.querySelectorAll(".ptr");

        index = (targetIndex < 1) ? slides.length : (targetIndex > slides.length) ? 1 : index;

        slides.forEach(slide => { slide.style.display = 'none';	});
        slides[index-1].style.display = 'block';

        if (pointers)
        {
            pointers.forEach(pointer => { pointer.className = pointer.className.replace(' active', '');	});
            pointers[index-1].className += ' active';
        }

    }

    function getSlideViewer()
    {
        createSlideViewContainer();
        createSlideviewPointerContainer();

        //console.log(progenitor.innerHTML);

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

            descendant.id = 'ceres-slideview-image-container';
            progenitor.appendChild(descendant);

            createAttribute(descendant.id, 'class', 'slideview-image-container');

            for (let item = 0; item < attributes.length; item++)
            {
                var arrayItem = ar[item].split(',');
                var qualifier = item + 1;

                let svcname = 'slideview' + qualifier;
                let surName = 'slideview-sur' + qualifier;
                let subName = 'slideview-sub' + qualifier;
                let imgName = 'slideview-img' + qualifier;

                setDivElement(svcname, 'slideview fade', descendant, null);

                progeny = document.getElementById(svcname);

                setDivElement(surName, 'surtitle', progeny, getSurtitle());
                setImgElement(imgName, 'ceres.openImageTab(this);', progeny);
                setDivElement(subName, 'subtitle', progeny, getSubtitle());
            }

            setAElement('slideview-prev', 'prev', 'ceres.getSlide(-1)', descendant, '&#10094;');
            setAElement('slideview-next', 'next', 'ceres.getSlide(1)', descendant, '&#10095;');

            function getURL()
            {
                return (arrayItem[0]) ? arrayItem[0] : null;
            }

            function getSurtitle()
            {
                return (sur) ? qualifier + ' / ' + attributes.length : null;
            }

            function getSubtitle()
            {
                return (sub) ? (arrayItem[1]) ? arrayItem[1] : null : null;
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
            progenitor.appendChild(document.createElement('br'));

            const descendant = document.createElement('div');

            descendant.id = 'ceres-slideview-pointer-container';
            progenitor.appendChild(descendant);

            createAttribute(descendant.id, 'class', 'slideview-pointer-container');

            for (let item = 0; item < attributes.length; item++)
            {
                var qualifier = item + 1;
                let svpname = 'slideview-ptr' + qualifier;

                setSpanElement(svpname, 'ptr', getClickEventValue(), descendant);
            }

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

    }

})(ceres);
