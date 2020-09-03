var index = 1;

function getSlide(n)
{
    showSlidesNoRepeat(index += n);
}

function setSlide(n)
{
    showSlidesNoRepeat(index = n);
}

function showSlidesNoRepeat(n = 1)
{
    let slides = document.getElementsByClassName('slideview');
    let dots = document.getElementsByClassName('dot');

    if (n > slides.length) index = 1;

    if (n < 1) index = slides.length;

    for (let i = 0; i < slides.length; i++) { slides[i].style.display = 'none'; }
    for (let i = 0; i < dots.length; i++) { dots[i].className = dots[i].className.replace(' active', ''); }

    slides[index-1].style.display = 'block';
    dots[index-1].className += ' active';
}

function showSlidesRepeat()
{
    let slides = document.getElementsByClassName('slideview');

    for (let i = 0; i < slides.length; i++) { slides[i].style.display = 'none'; }

    index++;

    if (index > slides.length) index = 1;
    slides[index-1].style.display = 'block';

    setTimeout(showSlidesRepeat, 2000); // Change image every 2 seconds
}

function relocateImage(el)
{
    window.open(el.getAttribute('src'), 'Image');
}
