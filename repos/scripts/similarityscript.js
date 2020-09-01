customElements.define('include-directive', class extends HTMLElement
{
  async connectedCallback()
  {
    let src = this.getAttribute('src');
    this.innerHTML = await (await fetch(src)).text();
  }

});

function getQueryString()
{
  const urlParams = new URLSearchParams(window.location.search);
  const mdd = urlParams.get('mdd')

  if (mdd != null) selectMarkdownDocument(mdd);
}

function onloadPrimary()
{
  getQueryString();
}

function scrollEventListener()
{
  window.onscroll = function(){ adjustHeaderDisplay(); };
}

function adjustHeaderDisplay()
{
  let el = parent.document.getElementById('site-header-display');

  if (document.body.scrollTop > 350 || document.documentElement.scrollTop > 350)
  {
    if (el.style.display != 'none') setTimeout(function(){ resetDisplay('none'); }, 100);

  } else {

    if (el.style.display === 'none') setTimeout(function(){ resetDisplay('block'); }, 500);

  }

  function resetDisplay(attribute)
  {
    el.style.display = attribute;
  }

}

function isValidSource(md)
{
    if (parent.document.getElementById('primary-container')) return true;

    window.location.href = 'https://ceresbakalite.github.io/similarity/?mdd=' + md;

    return false;
}

function onloadFrameComplete(ms, md)
{
  if (isValidSource(md))
  {
    scrollEventListener();

    switch (md)
    {
      case 'index':
        setTimeout(function(){ waitForMarkdown('index-md'); }, ms);
        break;

      case 'shell':
        setTimeout(function(){ waitForMarkdown('shell-md'); }, ms);
        break;

      case 'repos':
        setTimeout(function(){ waitForMarkdown('repos-md'); }, ms);
        break;

      default:
        setTimeout(function(){ waitForMarkdown('index-md'); }, ms);
        break;

    }

  }

}

function selectMarkdownDocument(md)
{
  switch (md)
  {
    case 'index':
      getMarkdownDocument('index', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncIndex.html');
      break;

    case 'shell':
      getMarkdownDocument('shell', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncShell.html');
      break;

    case 'repos':
      getMarkdownDocument('repos', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncRepos.html');
      break;

    default:
      getMarkdownDocument('index', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncIndex.html');
      break;

  }

}

function getMarkdownDocument(id, target)
{
    document.getElementById('frame-container').setAttribute('src', target);
    document.getElementById(id).blur();
}

function waitForMarkdown(target)
{
    document.getElementById('site-footer-display').style.display = 'block';
    document.getElementById('footer-content').style.display = 'block';

    refreshMarkdown(target);
}

function refreshMarkdown(target)
{
  if (document.getElementById(target))
  {
    document.getElementById(target).setAttribute('src', document.getElementById(target).getAttribute('src') + '?' + getRandomInteger(10000,1000000));

  } else if (document.getElementsByTagName('zero-md')[0]) {

    document.getElementsByTagName('zero-md')[0].setAttribute('src', document.getElementsByTagName('zero-md')[0].getAttribute('src') + '?' + getRandomInteger(10000,1000000));

  }

}

function getRandomInteger(min, max)
{
  return Math.floor(Math.random() * (max - min) ) + min;
}
