<div id="site-navigation-display" class="btn-group">
  <button id="slide-navbar" class="button" title="Slider screen views" onclick="similarity.getMarkupDocument('slide', this.id);">Views</button>
  <button id="shell-navbar" class="button" title="Command Line usage" onclick="similarity.getMarkupDocument('shell', this.id)">Shell</button>
  <button id="repos-navbar" class="button" title="Similarity version release notes" onclick="similarity.getMarkupDocument('repos', this.id)">Releases</button>
  <button id="index-navbar" class="button" title="Similarity" onclick="similarity.getMarkupDocument('index', this.id)">Home</button>
  <img id="pin-navbar" class="pin-navbar" title="Toggle pin status" state="disabled" onclick="similarity.resetPinState();" src="https://ceresbakalite.github.io/similarity/images/NAVPinIconDisabled.png">
</div>
