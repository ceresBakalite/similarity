var viewIndex = 1;

// Next/previous controls
function getSlide(n)
{
  showSlidesNoRepeat(viewIndex += n);
}

// Thumbnail image controls
function currentSlide(n)
{
  showSlidesNoRepeat(viewIndex = n);
}

function showSlidesNoRepeat(n = 1)
{
  let slides = document.getElementsByClassName('slideview');
  let dots = document.getElementsByClassName('dot');

  if (n > slides.length) viewIndex = 1;

  if (n < 1) viewIndex = slides.length;

  for (var i = 0; i < slides.length; i++) { slides[i].style.display = 'none'; }
  for (var i = 0; i < dots.length; i++) { dots[i].className = dots[i].className.replace(' active', ''); }

  slides[viewIndex-1].style.display = 'block';
  dots[viewIndex-1].className += ' active';
}

function showSlidesRepeat()
{
  let slides = document.getElementsByClassName('slideview');

  for (var i = 0; i < slides.length; i++) { slides[i].style.display = 'none'; }

  viewIndex++;

  if (viewIndex > slides.length) viewIndex = 1;
  slides[viewIndex-1].style.display = 'block';

  setTimeout(showSlidesRepeat, 2000); // Change image every 2 seconds
}

function relocateImage(el)
{
  alert(el.getAttribute('src'));
    window.open(el.getAttribute('src'), 'Image');
}
