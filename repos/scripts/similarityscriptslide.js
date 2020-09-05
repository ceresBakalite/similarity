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

    slideview.getSlide = function(targetIndex) { startSlideViewer(index += targetIndex); };

    slideview.setSlide = function(targetIndex) { startSlideViewer(index = targetIndex); };

    slideview.slideViewer = function()
    {
        buildSlideViewer();
        startSlideViewer();
    }

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

    function buildSlideViewer()
    {
        let ar = getSlideViewerAttributes();

        for (let el = 0; el < ar.length; ++el)
        {
            createCeresSlideviewImageContainer(el);
        }

        createSlideviewContainer();

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
            alert(css + ' - ' + ptr + ' - ' + sur + ' - ' + sub);
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
