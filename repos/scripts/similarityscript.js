function getMarkdown(ms)
{
  markdowntarget_shell();
  var interval = setInterval(waitformarkdown, ms);
}

function waitformarkdown()
{
  document.getElementById("site-footer-display").style.display = "block";
}

function markdowntarget_shell()
{
  alert('hello');
  var zeromd = document.getElementsByTagName("zero-md")[0];
  var attribute = document.createAttribute("file");

  attribute.value = "https://ceresbakalite.github.io/similarity/shell/README.md?" + getRandomInteger(10000,1000000);

alert(attribute.value);

  zeromd.setAttributeNode(attribute);
}

function getRandomInteger(min, max)
{
  return Math.floor(Math.random() * (max - min) ) + min;
}
