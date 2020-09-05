var ceres = {};
(function(slideview)
{
    var index = 1;
    var ptr = true; // default - display slideviewer pointers
    var sub = true; // default - display slideviewer subtitles
    var sur = true; // default - display slideviewer surtitles
    var css = true; // default - use slideviewer css

    window.customElements.define('ceres-slideview', class extends HTMLElement
    {
        async connectedCallback()
        {
            let src = this.getAttribute('src');
            this.innerHTML = await (await fetch(src)).text();
        }

    });

    slideview.openImageTab = function(el) { window.open(el.getAttribute('src'), 'image'); };

    slideview.getSlide = function(targetIndex) { slideview.startSlideViewer(index += targetIndex); };

    slideview.setSlide = function(targetIndex) { slideview.startSlideViewer(index = targetIndex); };

    slideview.startSlideViewer = function(targetIndex)
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

    slideview.imageListToArray = function(str)
    {
        return str.replace(/((<([^>]+)>)| )/gi, '').trim().replace(/\r\n|\r|\n|,/gi, ';').split(';');
    }

    slideview.getSlideViewerAttributes = function()
    {
        let el = (document.getElementById("ceres-slideview")) ? document.getElementById("ceres-slideview") : document.getElementsByTagName('ceres-slideview')[0];

        ptr = (el.getAttribute('ptr')) ? el.getAttribute('ptr') : ptr;
        sub = (el.getAttribute('sub')) ? el.getAttribute('sub') : sub;
        sur = (el.getAttribute('sur')) ? el.getAttribute('sur') : sur;
        css = (el.getAttribute('css')) ? el.getAttribute('css') : css;

        return slideview.imageListToArray(el.innerHTML);
    }

    slideview.buildSlideViewer = function()
    {
        let ar = slideview.getSlideViewerAttributes();

        for (let el = 0; el < ar.length; ++el)
        {
            //alert(ar[el].trim());
        }

    }

})(ceres);
