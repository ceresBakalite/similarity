var ceres = {};
(function(slideview)
{
    let progenitor = null; // parent slideview place holder
    let attributes = null; // slideview element item attributes array

    let ptr = true; // default element attribute - display slideview item pointers
    let sub = true; // default element attribute - display slideview item subtitles
    let sur = true; // default element attribute - display slideview item surtitles
    let css = true; // default element attribute - use the default slideview stylesheet
    let trc = false; // default element attribute - enable the trace environment directive

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
        attributes = getSlideViewAttributes();

        getSlideViewer();

        function getSlideViewAttributes()
        {
            ptr = (progenitor.getAttribute('ptr')) ? progenitor.getAttribute('ptr') : ptr;
            sub = (progenitor.getAttribute('sub')) ? progenitor.getAttribute('sub') : sub;
            sur = (progenitor.getAttribute('sur')) ? progenitor.getAttribute('sur') : sur;
            css = (progenitor.getAttribute('css')) ? progenitor.getAttribute('css') : css;
            trc = (progenitor.getAttribute('trc')) ? progenitor.getAttribute('trc') : trc;

alert('bool: ' + progenitor.innerHTML.length);

            let imageList = (progenitor.innerHTML.length > 0) ? progenitor.innerHTML : getImageList();

            return imageListToArray(imageList);

            function imageListToArray(str)
            {
                return str.replace(/((<([^>]+)>)| )/gi, '').trim().replace(/\r\n|\r|\n/gi, ';').split(';');
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

        if (pointers)
        {
            pointers.forEach(pointer => { pointer.className = pointer.className.replace(' active', '');	});
            pointers[index-1].className += ' active';
        }

    }

    function getSlideViewer()
    {
        if (css) linkSlideViewerCSS();

        createSlideViewContainer();
        createSlideviewPointerContainer();

        if (trc) console.log(progenitor.innerHTML);

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
            if (trc) console.log(progenitor.innerHTML);

            progenitor.innerHTML = null;

            const descendant = document.createElement('div');
            let progeny = null;

            descendant.id = 'ceres-slideview-image-container';
            progenitor.appendChild(descendant);

            createAttribute(descendant.id, 'class', 'slideview-image-container');

            for (let item = 0; item < attributes.length; item++)
            {
                var arrayItem = attributes[item].split(',');
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

            progenitor.appendChild(document.createElement('br'));

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

        function linkSlideViewerCSS()
        {
            const link = document.createElement('link');

            link.rel = 'stylesheet';
            link.type = 'text/css';
            link.href = 'https://ceresbakalite.github.io/similarity/stylesheets/similaritysheetslide.css';
            link.as = 'style';

            link.onload = function ()
            {
                if (trc) console.log('onload listener');
            }

            if (link.addEventListener)
            {
                link.addEventListener('load', function()
                {
                    if (trc) console.log("DOM's load event");
                }, false);

            }

            link.onreadystatechange = function()
            {
                var state = link.readyState;

                if (state === 'loaded' || state === 'complete')
                {
                    link.onreadystatechange = null;
                    if (trc) console.log('onreadystatechange');
                }

            };

            var cssnum = document.styleSheets.length;

            var ti = setInterval(function()
            {
                if (document.styleSheets.length > cssnum)
                {
                    clearInterval(ti);
                    if (trc) console.log('listening to styleSheets.length change');
                }

            }, 10);

            document.head.appendChild(link);
        }

    }

})(ceres);
