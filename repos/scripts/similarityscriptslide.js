var ceres = {};
(function(slideview)
{
    let index = 1;
    let css = true; // default - use slideviewer css
    let ptr = true; // default - display slideviewer pointers
    let sur = true; // default - display slideviewer surtitles
    let sub = true; // default - display slideviewer subtitles

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
        buildSlideViewer();
        displaySlide();
    }

    function displaySlide(targetIndex)
    {
        const slides = document.querySelectorAll(".slideview");
        const pointers = document.querySelectorAll(".ptr");

        slides.forEach(slide => { slide.style.display = 'none';	});
        pointers.forEach(pointer => { pointer.className = pointer.className.replace(' active', '');	});

        index = (targetIndex < 1) ? slides.length : (targetIndex > slides.length) ? 1 : index;

        slides[index-1].style.display = 'block';
        pointers[index-1].className += ' active';
    };

    function buildSlideViewer()
    {
        let ar = getSlideViewerAttributes();
        let progenitor = document.getElementById("slideview");

        //createCeresSlideviewImageContainer();

        function getSlideViewerAttributes()
        {
            let el = (document.getElementById("ceres-slideview")) ? document.getElementById("ceres-slideview") : document.getElementsByTagName('ceres-slideview')[0];

            ptr = (el.getAttribute('ptr')) ? el.getAttribute('ptr') : ptr;
            sub = (el.getAttribute('sub')) ? el.getAttribute('sub') : sub;
            sur = (el.getAttribute('sur')) ? el.getAttribute('sur') : sur;
            css = (el.getAttribute('css')) ? el.getAttribute('css') : css;

            return imageListToArray(el.innerHTML);

            function imageListToArray(str)
            {
                return str.replace(/((<([^>]+)>)| )/gi, '').trim().replace(/\r\n|\r|\n/gi, ';').split(';');
            }

        }

        // create ceres-slideview-image-container and class
        function createCeresSlideviewImageContainer()
        {
            //alert(ar[el].trim());
            let parent = document.createElement('div');

            parent.createAttribute('id');
            parent.createAttribute('class');
            parent.setAttribute('id', 'ceres-slideview-image-container');
            parent.setAttribute('class', 'slideview-image-container');

            progenitor.appendChild(parent);

            ar.forEach(item =>
            {
                let urlArray = item.value.split(',');
                let slideview-name = 'slideview' + item;
                let slideview-sur-name = 'slideview-sur' + item;
                let slideview-sub-name = 'slideview-sub' + item;
                let slideview-img-name = 'slideview-img' + item;

                setDivElement(slideview-name, 'slideview fade', parent, null);

                child = document.getElementById(slideview-name);

                setDivElement(slideview-sur-name, 'surtitle', child, getSurtitle());
                setImgElement(slideview-img-name, 'ceres.openImageTab(this);', child);
                setDivElement(slideview-sub-name, 'subtitle', child, getSubtitle());
            });

            function getSurtitle()
            {
                return (sur) ? item + ' / ' + ar.Length : null;
            }

            function getSubtitle()
            {
                if (!sub) return null;
                return (urlArray[1] != null) ? urlArray[1] : null;
            }

            // create slideview+n and class append child
            function setDivElement(idStr, classStr, obj, htmlStr)
            {
                let el = document.createElement('div');

                el.createAttribute('id');
                el.createAttribute('class');
                el.setAttribute('id', idStr);
                el.setAttribute('class', classStr);

                if (htmlStr != null) el.innerHTML = htmlStr;

                obj.appendChild(el);

            }

            // create slideview-img+n and onclick event append child
            function setImgElement(idStr, clickStr, obj)
            {
                let el = document.createElement('img');

                el.createAttribute('id');
                el.createAttribute('onclick');
                el.createAttribute('src');
                el.setAttribute('id', 'slideview-img' + item);
                el.setAttribute('onclick', clickStr);
                el.setAttribute('src', urlArray[0]);

                obj.appendChild(el);
            }

        }


        // create br append child
        function createLineBreakContainer()
        {
        }

        // create ceres-slideview-pointer-container and class
        function createCeresSlideviewPointerContainer()
        {
        }

        // create slideview-ptr+n and class and onclick event append child
        function createSlideviewPointerContainer()
        {
        }

    }

})(ceres);
