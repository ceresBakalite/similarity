function markdownpause(ms)
{
  var interval = setInterval(waitformarkdown, ms);
}

function waitformarkdown()
{
  document.getElementById("site-footer-display").style.display = "block";
}
