var ceres = {};
(function(slideview)
{
    var index = 1;
    var ptr = true;
    var sub = true;
    var sur = true;
    var css = true;

    window.customElements.define('ceres-slideview', class extends HTMLElement
    {
        async connectedCallback()
        {
            let src = this.getAttribute('src');
            this.innerHTML = await (await fetch(src)).text();
        }

    });

    slideview.openImageTab = function(el) { window.open(el.getAttribute('src'), 'image'); };

    slideview.getSlide = function(i) { slideview.startSlideViewer(index += i); };

    slideview.setSlide = function(i) { slideview.startSlideViewer(index = i); };

    slideview.startSlideViewer = function(n)
    {
        const slides = document.querySelectorAll(".slideview");
        const pointers = document.querySelectorAll(".ptr");
        //let slides = document.getElementsByClassName('slideview');
        //let ptrs = document.getElementsByClassName('ptr');

        index = (n < 1) ? slides.length : (n > slides.length) ? 1 : index;

        for (let i = 0; i < slides.length; i++) { slides[i].style.display = 'none'; }
        for (let i = 0; i < pointers.length; i++) { pointers[i].className = pointers[i].className.replace(' active', ''); }

        slides[index-1].style.display = 'block';
        pointers[index-1].className += ' active';
    };

    slideview.slideViewer = function()
    {
        slideview.buildSlideViewer();
        slideview.startSlideViewer();
    }

    slideview.imageListToArray = function(str)
    {
        return str.replace(/((<([^>]+)>)| )/gi, '').trim().replace(/\r\n|\r|\n|,/gi, ';').split(';');
    }

    slideview.getSlideViewerAttributes = function()
    {
        let el = (document.getElementById("ceres-slideview")) ? document.getElementById("ceres-slideview") : document.getElementsByTagName('ceres-slideview')[0];

        ptr = (el.getAttribute('ptr')) ? el.getAttribute('ptr') : ptr; // default true - display slideviewer pointers
        sub = (el.getAttribute('sub')) ? el.getAttribute('sub') : sub; // default true - display slideviewer subtitles
        sur = (el.getAttribute('sur')) ? el.getAttribute('sur') : sur; // default true - display slideviewer surtitles
        css = (el.getAttribute('css')) ? el.getAttribute('css') : css; // default true - use slideviewer css

        return slideview.imageListToArray(el.innerHTML);
    }

    slideview.buildSlideViewer = function()
    {
        let ar = slideview.getSlideViewerAttributes();

        for (let el = 0; el < ar.length; ++el)
        {
            alert(ar[el].trim());
        }

    }

})(ceres);
