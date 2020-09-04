var bakaliteslider = new function()
{
    var slideInterval = 5000;
    var slideRepeatInterval = 25000;

    var slideIndex = 1;
    var slideLastSlideTime = getTimeNow();

    var getTimeNow = function()
    {
        return new Date().getTime();
    };

    var getSlide = function(n)
    {
        startSlideViewer(slideIndex += n);
    };

    var setSlide = function(n)
    {
        startSlideViewer(slideIndex = n);
    };

    var.startSlideViewer = function(n)
    {
        let slides = document.getElementsByClassName('slideview');
        let dots = document.getElementsByClassName('dot');

        slideIndex = (n < 1) ? slides.length : (n > slides.length) ? 1 : slideIndex;

        for (let i = 0; i < slides.length; i++) { slides[i].style.display = 'none'; }
        for (let i = 0; i < dots.length; i++) { dots[i].className = dots[i].className.replace(' active', ''); }

        slides[slideIndex-1].style.display = 'block';
        dots[slideIndex-1].className += ' active';
    };

    var openImageTab = function(el)
    {
        window.open(el.getAttribute('src'), 'image');
    };

};
