window.viInterop = {
    init: function (playerId, insightsId, context) {
        this.playerWidget = new window.vi.widgets.Player(
            playerId,
            { width: 560, height: 315, autoplay: false },
            {
                accountId: context.accountId,
                videoId: context.videoId,
                accessToken: context.accessToken,
                region: context.region
            }
        );
        this.playerWidget.render();

        this.insightsWidget = new window.vi.widgets.Insights(
            insightsId,
            { selectedTab: "timeline" },
            {
                accountId: context.accountId,
                videoId: context.videoId,
                accessToken: context.accessToken,
                region: context.region
            }
        );
        this.insightsWidget.render();

        this.insightsWidget.on('timeSelected', (time) => {
            this.playerWidget.seek(time);
        });
    }
};
