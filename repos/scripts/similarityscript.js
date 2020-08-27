function getMarkdown_Shell(ms)
{
  refreshMarkdown_Shell();
  var interval = setInterval(waitformarkdown, ms);
}

function refreshMarkdown_Shell()
{
  document.getElementsByTagName("zero-md")[0].setAttribute("file", "https://ceresbakalite.github.io/similarity/shell/README.md?" + getRandomInteger(10000,1000000));
}

function waitForMarkdown()
{
  document.getElementById("site-footer-display").style.display = "block";
}

function getRandomInteger(min, max)
{
  return Math.floor(Math.random() * (max - min) ) + min;
}
