function markdownpause(ms)
{
  var interval = setInterval(waitformarkdown, ms);
}

function waitformarkdown()
{
  // document.getElementById("zero-md-display").style.display = "block";
  document.getElementById("site-footer-display").style.display = "block";
}
