var ceresview = {}

ceresview.index = 1;

slideInterval = 5000;
slideRepeatInterval = 25000;

slideIndex = 1;
slideLastSlideTime = getTimeNow();

var getTimeNow = function()
{
    return new Date().getTime();
};

function getSlide(n)
{
    startSlideViewer(slideIndex += n);
};

function setSlide(n)
{
    startSlideViewer(slideIndex = n);
};

function startSlideViewer(n)
{
    alert(ceresview.index);

    ceresview.index++;

    alert(ceresview.index);

    let slides = document.getElementsByClassName('slideview');
    let dots = document.getElementsByClassName('dot');

    slideIndex = (n < 1) ? slides.length : (n > slides.length) ? 1 : slideIndex;

    for (let i = 0; i < slides.length; i++) { slides[i].style.display = 'none'; }
    for (let i = 0; i < dots.length; i++) { dots[i].className = dots[i].className.replace(' active', ''); }

    slides[slideIndex-1].style.display = 'block';
    dots[slideIndex-1].className += ' active';
};

function openImageTab(el)
{
    window.open(el.getAttribute('src'), 'image');
};
