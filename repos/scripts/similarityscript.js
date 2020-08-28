function getMarkdown(ms, target)
{
  switch (target)
  {
    case 'shell':
      refreshMarkdown_Shell();
      var result = setInterval(waitForMarkdown(null, false), ms);
      break;

    case 'repos':
      alert('repose');
      refreshMarkdown_Repos();
      var result = setInterval(waitForMarkdown("https://ceresbakalite.github.io/similarity/images/NAVSimilarityLogoRepos.png", true), ms);
      alert('end repose');
      break;

    default:
      refreshMarkdown_Shell();
  }

}

function refreshMarkdown_Shell()
{
  document.getElementsByTagName("zero-md")[0].setAttribute("file", "https://ceresbakalite.github.io/similarity/shell/README.md?" + getRandomInteger(10000,1000000));
}

function refreshMarkdown_Repos()
{
  document.getElementsByTagName("zero-md")[0].setAttribute("file", "https://ceresbakalite.github.io/similarity/README.md?" + getRandomInteger(10000,1000000));
}

function waitForMarkdown(url, resetlogo)
{
  if (resetlogo) resetMarkdown_Logo(url);
  document.getElementById("site-footer-display").style.display = "block";
}

function getRandomInteger(min, max)
{
  return Math.floor(Math.random() * (max - min) ) + min;
}

function resetMarkdown_Logo(url)
{
  document.getElementsByTagName("img")[1].setAttribute("src", url);
}
