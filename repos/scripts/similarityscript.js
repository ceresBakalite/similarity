customElements.define('include-directive', class extends HTMLElement
{
  async connectedCallback()
  {
    let src = this.getAttribute('src');
    this.innerHTML = await (await fetch(src)).text();
  }

});

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
  alert('-1');
  async connectedCallback()
  {
    alert('1');
    let el = document.getElementById('index-md');
    let src = 'https://ceresbakalite.github.io/similarity/repos/scripts/index.md?' + getRandomInteger(10000,1000000);
    el.src = await (await fetch(src)).text();
    alert('2');
  }

  alert('+2');

  //WebComponents.waitFor(() =>
  //{
  //    document.getElementsByTagName('zero-md')[0].setAttribute('src', 'https://ceresbakalite.github.io/similarity/repos/scripts/index.md?' + getRandomInteger(10000,1000000));
  //});

}

function refreshMarkdown_Shell()
{
  WebComponents.waitFor(() =>
  {
      document.getElementsByTagName('zero-md')[0].setAttribute('src', 'https://ceresbakalite.github.io/similarity/shell/README.md?' + getRandomInteger(10000,1000000));
  });

}

function refreshMarkdown_Repos()
{
  WebComponents.waitFor(() =>
  {
      document.getElementsByTagName('zero-md')[0].setAttribute('src', 'https://ceresbakalite.github.io/similarity/README.md?' + getRandomInteger(10000,1000000));
  });

}

function waitForMarkdown()
{
  WebComponents.waitFor(() =>
  {
      document.getElementById('site-footer-display').style.display = 'block';
  });

}

function getRandomInteger(min, max)
{
  return Math.floor(Math.random() * (max - min) ) + min;
}
