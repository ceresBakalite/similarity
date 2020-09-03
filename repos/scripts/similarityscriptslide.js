var slideIndex = 1;

// Next/previous controls
function plusSlides(n)
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
  var slides = document.getElementsByClassName("mySlides");
  var dots = document.getElementsByClassName("dot");

  if (n > slides.length) slideIndex = 1;

  if (n < 1) slideIndex = slides.length;

  for (var i = 0; i < slides.length; i++) { slides[i].style.display = "none"; }
  for (var i = 0; i < dots.length; i++) { dots[i].className = dots[i].className.replace(" active", ""); }

  slides[slideIndex-1].style.display = "block";
  dots[slideIndex-1].className += " active";
}

function showSlidesRepeat()
{
  var slides = document.getElementsByClassName("mySlides");

  for (var i = 0; i < slides.length; i++) { slides[i].style.display = "none"; }

  slideIndex++;

  if (slideIndex > slides.length) slideIndex = 1;
  slides[slideIndex-1].style.display = "block";

  setTimeout(showSlidesRepeat, 2000); // Change image every 2 seconds
}
