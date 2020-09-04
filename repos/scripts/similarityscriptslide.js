const slideInterval = 5000;
const slideRepeatInterval = 25000;

var slideIndex = 1;
var slideLastSlideTime = getTimeNow();

function getTimeNow()
{
    return new Date().getTime();
}

function getSlide(n)
{
 //   setInterval(startSlideViewer, slideInterval);
    startSlideViewer(slideIndex += n);
}

function setSlide(n)
{
 //   setInterval(startSlideViewer, slideInterval);
    startSlideViewer(slideIndex = n);
}

function startSlideViewer(n)
{
    let slides = document.getElementsByClassName('slideview');
    let dots = document.getElementsByClassName('dot');

    slideIndex = (n < 1) ? slides.length : (n > slides.length) ? 1 : slideIndex;

    for (let i = 0; i < slides.length; i++) { slides[i].style.display = 'none'; }
    for (let i = 0; i < dots.length; i++) { dots[i].className = dots[i].className.replace(' active', ''); }

    slides[slideIndex-1].style.display = 'block';
    dots[slideIndex-1].className += ' active';
}

function openImageTab(el)
{
    window.open(el.getAttribute('src'), 'image');
}
