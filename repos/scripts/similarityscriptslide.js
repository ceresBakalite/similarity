var ceresview = {}

ceresview.slideInterval = 5000;
ceresview.slideRepeatInterval = 25000;

ceresview.slideIndex = 1;
ceresview.slideLastSlideTime = ceresview.getTimeNow();

ceresview.getTimeNow = function()
{
    return new Date().getTime();
};

function getSlide(n)
{
    startSlideViewer(ceresview.slideIndex += n);
};

function setSlide(n)
{
    startSlideViewer(ceresview.slideIndex = n);
};

function startSlideViewer(n)
{
    let slides = document.getElementsByClassName('slideview');
    let dots = document.getElementsByClassName('dot');

    ceresview.slideIndex = (n < 1) ? slides.length : (n > slides.length) ? 1 : ceresview.slideIndex;

    for (let i = 0; i < slides.length; i++) { slides[i].style.display = 'none'; }
    for (let i = 0; i < dots.length; i++) { dots[i].className = dots[i].className.replace(' active', ''); }

    slides[ceresview.slideIndex-1].style.display = 'block';
    dots[ceresview.slideIndex-1].className += ' active';
};

function openImageTab(el)
{
    window.open(el.getAttribute('src'), 'image');
};
