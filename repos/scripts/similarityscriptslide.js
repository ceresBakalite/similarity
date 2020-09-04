var ceresbakalite = {}

ceresbakalite.index = 1;
ceresbakalite.slideInterval = 5000;
ceresbakalite.slideRepeatInterval = 25000;
ceresbakalite.slideLastSlideTime = ceresbakalite.getTimeNow();

ceresbakalite.getTimeNow = function()
{
    return new Date().getTime();
};

ceresbakalite.getSlide function(n)
{
    startSlideViewer(ceresbakalite.index += n);
};

function setSlide(n)
{
    startSlideViewer(ceresbakalite.index = n);
};

function startSlideViewer(n)
{
    let slides = document.getElementsByClassName('slideview');
    let dots = document.getElementsByClassName('dot');

    ceresbakalite.index = (n < 1) ? slides.length : (n > slides.length) ? 1 : ceresbakalite.index;

    for (let i = 0; i < slides.length; i++) { slides[i].style.display = 'none'; }
    for (let i = 0; i < dots.length; i++) { dots[i].className = dots[i].className.replace(' active', ''); }

    slides[ceresbakalite.index-1].style.display = 'block';
    dots[ceresbakalite.index-1].className += ' active';
};

function openImageTab(el)
{
    window.open(el.getAttribute('src'), 'image');
};
