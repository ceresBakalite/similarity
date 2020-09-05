var ceres = {};
(function(slideview)
{
    var index = 1;
    var dot = true;
    var txt = true;
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
        let slides = document.getElementsByClassName('slideview');
        let dots = document.getElementsByClassName('dot');

        index = (n < 1) ? slides.length : (n > slides.length) ? 1 : index;

        for (let i = 0; i < slides.length; i++) { slides[i].style.display = 'none'; }
        for (let i = 0; i < dots.length; i++) { dots[i].className = dots[i].className.replace(' active', ''); }

        slides[index-1].style.display = 'block';
        dots[index-1].className += ' active';
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

    slideview.getSlideViewAttributes = function()
    {
        let el = (document.getElementById("ceres-slideview")) ? document.getElementById("ceres-slideview") : document.getElementsByTagName('ceres-slideview')[0];

        dot = (el.getAttribute('dot')) ? el.getAttribute('dot') : dot;
        txt = (el.getAttribute('txt')) ? el.getAttribute('txt') : txt;
        css = (el.getAttribute('css')) ? el.getAttribute('css') : css;

        return slideview.imageListToArray(el.innerHTML);
    }

    slideview.buildSlideViewer = function()
    {
        let ar = slideview.getSlideViewAttributes();

        for (let el = 0; el < ar.length; ++el)
        {
            //alert(ar[el].trim());
        }

    }

})(ceres);
