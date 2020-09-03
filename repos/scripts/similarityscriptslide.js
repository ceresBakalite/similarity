var slideIndex = 1;

// Next/previous controls
function getSlide(n)
{
  showSlidesNoRepeat(slideIndex += n);
}

// Thumbnail image controls
function currentSlide(n)
{
  showSlidesNoRepeat(slideIndex = n);
}

function showSlidesNoRepeat(n = 1)
{
  let slides = document.getElementsByClassName("mySlides");
  let dots = document.getElementsByClassName("dot");

  if (n > slides.length) slideIndex = 1;

  if (n < 1) slideIndex = slides.length;

  for (let i = 0; i < slides.length; i++) { slides[i].style.display = "none"; }
  for (let i = 0; i < dots.length; i++) { dots[i].className = dots[i].className.replace(" active", ""); }

  slides[slideIndex-1].style.display = "block";
  dots[slideIndex-1].className += " active";
}

function showSlidesRepeat()
{
  let slides = document.getElementsByClassName("mySlides");

  for (let i = 0; i < slides.length; i++) { slides[i].style.display = "none"; }

  slideIndex++;

  if (slideIndex > slides.length) slideIndex = 1;
  slides[slideIndex-1].style.display = "block";

  setTimeout(showSlidesRepeat, 2000); // Change image every 2 seconds
}

function relocateImage(id)
{
    window.open(location.href = document.getElementById(id).getAttribute('src'), '_blank');
}
