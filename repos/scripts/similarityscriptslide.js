var slideview = {}
slideview.index = 1;

slideview.openImageTab = function(el)
{
    window.open(el.getAttribute('src'), 'image');
}

slideview.getSlide = function(i)
{
    slideview.startSlideViewer(slideview.index += i);
}

slideview.setSlide = function(i)
{
    slideview.startSlideViewer(slideview.index = i);
}

slideview.startSlideViewer = function(n)
{
    alert('hello');
    let slides = document.getElementsByClassName('slideview');
    let dots = document.getElementsByClassName('dot');

    slideview.index = (n < 1) ? slides.length : (n > slides.length) ? 1 : slideview.index;

    for (let i = 0; i < slides.length; i++) { slides[i].style.display = 'none'; }
    for (let i = 0; i < dots.length; i++) { dots[i].className = dots[i].className.replace(' active', ''); }

    slides[slideview.index-1].style.display = 'block';
    dots[slideview.index-1].className += ' active';
}
