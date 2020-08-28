function getMarkdown(ms, target)
{
  switch (target)
  {
    case 'index':
      refreshMarkdown_Index();
      break;

    case 'shell':
      refreshMarkdown_Shell();
      break;

    case 'repos':
      refreshMarkdown_Repos();
      break;

    default:
      refreshMarkdown_Shell();
  }

  setInterval(waitForMarkdown, ms);
}

function refreshMarkdown_Index()
{
  WebComponents.waitFor(() =>
  {
    let el = document.getElementsByTagName('zero-md')[0];
    el.setAttribute('src') = 'https://ceresbakalite.github.io/similarity/repos/scripts/index.md?' + getRandomInteger(10000,1000000);
  });

}

function refreshMarkdown_Shell()
{
  WebComponents.waitFor(() =>
  {
    let el = document.getElementsByTagName('zero-md')[0];
    el.setAttribute('src') = 'https://ceresbakalite.github.io/similarity/shell/README.md?' + getRandomInteger(10000,1000000);
  });

}

function refreshMarkdown_Repos()
{
  WebComponents.waitFor(() =>
  {
    let el = document.getElementsByTagName('zero-md')[0];
    el.setAttribute('src') = 'https://ceresbakalite.github.io/similarity/README.md?' + getRandomInteger(10000,1000000));
  });

}

function waitForMarkdown()
{
  WebComponents.waitFor(() =>
  {
    let el = document.getElementById('site-footer-display');
    el.style.display = 'block';
  });

}

function getRandomInteger(min, max)
{
  return Math.floor(Math.random() * (max - min) ) + min;
}
