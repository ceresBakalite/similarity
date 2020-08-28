function getMarkdown(ms, target)
{
  switch (target)
  {
    case 'shell':
      refreshMarkdown_Shell();
      break;

    case 'repos':
      refreshMarkdown_Repos();
      break;

    default:
      refreshMarkdown_Shell();
  }

  var interval = setInterval(waitForMarkdown, ms);
}

function refreshMarkdown_Shell()
{
  alert('shell 1');
  document.getElementsByTagName("zero-md")[0].setAttribute("file", "https://ceresbakalite.github.io/similarity/shell/README.md?" + getRandomInteger(10000,1000000));
  alert('shell 2');
}

function refreshMarkdown_Repos()
{
  alert('repos 1');
  document.getElementsByTagName("zero-md")[0].setAttribute("file", "https://ceresbakalite.github.io/similarity/README.md?" + getRandomInteger(10000,1000000));
  alert('repos 2');
  document.getElementsByTagName("img")[0].setAttribute("src", "https://ceresbakalite.github.io/similarity/images/NAVSimilarityLogoRepos.png");
  alert('repos 3');
}

function waitForMarkdown()
{
  document.getElementById("site-footer-display").style.display = "block";
}

function getRandomInteger(min, max)
{
  return Math.floor(Math.random() * (max - min) ) + min;
}
