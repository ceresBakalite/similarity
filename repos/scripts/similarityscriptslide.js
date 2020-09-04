const slideInterval = 2000;
const slideRepeatInterval = 10000;

var slideIndex = 0;
var slideLastSlideTime = getTimeNow();
var slideRepeat = setTimeout(startSlideViewerRepeat, slideInterval);

function getTimeNow()
{
    return new Date().getTime();
}

function getSlide(n)
{
    startSlideViewer(slideIndex += n);
}

function setSlide(n)
{
    startSlideViewer(slideIndex = n);
}

function startSlideViewer(n = 1)
{
    checkElapsedTime();
    cancelSlideRepeat()

    let slides = document.getElementsByClassName('slideview');
    let dots = document.getElementsByClassName('dot');

    slideIndex = (n < 1) ? slides.length : (n > slides.length) ? 1 : slideIndex;

    for (let i = 0; i < slides.length; i++) { slides[i].style.display = 'none'; }
    for (let i = 0; i < dots.length; i++) { dots[i].className = dots[i].className.replace(' active', ''); }

    slides[slideIndex-1].style.display = 'block';
    dots[slideIndex-1].className += ' active';

    slideRepeat = setTimeout(startSlideViewerRepeat, slideInterval);
}

function startSlideViewerRepeat()
{
    let slides = document.getElementsByClassName('slideview');
    let dots = document.getElementsByClassName('dot');

    for (let i = 0; i < slides.length; i++) { slides[i].style.display = 'none'; }
    for (let i = 0; i < dots.length; i++) { dots[i].className = dots[i].className.replace(' active', ''); }

    slideIndex++;

    if (slideIndex > slides.length) slideIndex = 1;

    slides[slideIndex-1].style.display = 'block';
    dots[slideIndex-1].className += ' active';

    slideRepeat = setTimeout(startSlideViewerRepeat, slideInterval);
}

function openImageTab(el)
{
    window.open(el.getAttribute('src'), 'image');
}

function cancelSlideRepeat()
{
    clearTimeout(slideRepeat);
    slideLastSlideTime = getTimeNow();
}

function checkElapsedTime()
{
    if ((getTimeNow() - slideLastSlideTime) > slideRepeatInterval) startSlideViewerRepeat();
}
