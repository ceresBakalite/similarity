var ceres = {};
(function(slideview)
{
    var index = 1;
    var css = true; // default - use slideviewer css
    var ptr = true; // default - display slideviewer pointers
    var sur = true; // default - display slideviewer surtitles
    var sub = true; // default - display slideviewer subtitles

    window.customElements.define('ceres-slideview', class extends HTMLElement
    {
        async connectedCallback()
        {
            let src = this.getAttribute('src');
            this.innerHTML = await (await fetch(src)).text();
        }

    });

    slideview.openImageTab = function(el) { window.open(el.getAttribute('src'), 'image'); };

    slideview.getSlide = function(targetIndex) { startSlideViewer(index += targetIndex); };

    slideview.setSlide = function(targetIndex) { startSlideViewer(index = targetIndex); };

    function startSlideViewer(targetIndex)
    {
        const slides = document.querySelectorAll(".slideview");
        const pointers = document.querySelectorAll(".ptr");

        slides.forEach(slide => { slide.style.display = 'none';	});
        pointers.forEach(pointer => { pointer.className = pointer.className.replace(' active', '');	});

        index = (targetIndex < 1) ? slides.length : (targetIndex > slides.length) ? 1 : index;

        slides[index-1].style.display = 'block';
        pointers[index-1].className += ' active';
    };

    slideview.slideViewer = function()
    {
        slideview.buildSlideViewer();
        slideview.startSlideViewer();
    }

    slideview.buildSlideViewer = function()
    {
        let ar = getSlideViewerAttributes();

        for (let el = 0; el < ar.length; ++el)
        {
            createCeresSlideviewImageContainer(el);
        }

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
        function createCeresSlideviewImageContainer(el)
        {
            alert(ar[el].trim());
        }

        // create slideview+n and class append child
        function createSlideviewContainer()
        {
        }

        // create slideview-sur+n and class append child
        function createSlideviewSurtitleContainerfunction()
        {
        }

        // create slideview-img+n and onclick event append child
        function createSlideviewImageContainer()
        {
        }

        // create slideview-sub+n and class append child
        function createSlideviewSubtitleContainer()
        {
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
