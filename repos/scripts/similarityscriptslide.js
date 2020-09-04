var ceresbakalite = {}
ceresbakalite.index = 1;

function getSlide(index)
{
    startSlideViewer(ceresbakalite.index += index);
};

function setSlide(index)
{
    startSlideViewer(ceresbakalite.index = index);
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
