var ceres = {};
(function(slideview)
{
    var index = 1;

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

    slideview.getImageList = function()
    {
        let el = (document.getElementById("ceres")) ? document.getElementById("ceres") : document.getElementsByTagName('ceres')[0];
        return el.getAttribute('src').split(',');
    }

    slideview.buildSlideViewer = function()
    {
        let ar = slideview.getImageList();

        for (let el = 0; el < ar.length; ++el)
        {
            alert(ar[el]);
        }

    }

})(ceres);
