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

function //relocateImage(id)
{
    window.open(location.href = document.getElementById(id).getAttribute('src'), 'Image');
}

function relocateImage(id) {
   var largeImage = document.getElementById(id);
   largeImage.style.display = 'block';
   largeImage.style.width=200+"px";
   largeImage.style.height=200+"px";
   var url=largeImage.getAttribute('src');
   window.open(url,'Image','width=largeImage.stylewidth,height=largeImage.style.height,resizable=1');
}
