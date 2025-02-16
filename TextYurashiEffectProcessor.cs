using Vortice.Direct2D1;
using Vortice.DirectWrite;
using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Player.Video;

namespace TextYurashiPlugin
{
    internal class TextYurashiEffectProcessor : IVideoEffectProcessor
    {
        readonly TextYurashiEffect item;
        ID2D1Image? input;

        public ID2D1Image Output => input ?? throw new NullReferenceException(nameof(input) + "is null");

        public TextYurashiEffectProcessor(TextYurashiEffect item)
        {
            this.item = item;
        }

        /// <summary>
        /// エフェクトを更新する
        /// </summary>
        /// <param name="effectDescription">エフェクトの描画に必要な各種情報</param>
        /// <returns>描画位置等の情報</returns>
        public DrawDescription Update(EffectDescription effectDescription)
        {
            var frame = effectDescription.ItemPosition.Frame;
            var length = effectDescription.ItemDuration.Frame;
            var fps = effectDescription.FPS;
            var x = item.X.GetValue(frame, length, fps);
            var y = item.Y.GetValue(frame, length, fps);
            var 間隔 = item.間隔.GetValue(frame, length, fps);
            var drawDesc = effectDescription.DrawDescription;
            var seeds = frame + drawDesc.Draw.X + drawDesc.Draw.Y;
            var fps_frame = frame % fps;
            var fps間隔 = fps_frame % (fps / Math.Round(1 / 間隔));
            if (fps間隔 != 0) {
                Random random2 = new Random((int)seeds - (int)fps間隔);
                return
                    drawDesc with
                    {
                        Draw = new(
                        drawDesc.Draw.X + random2.Next(Math.Abs((int)x)) - 50,
                        drawDesc.Draw.Y + random2.Next(Math.Abs((int)y)) - 50,
                        drawDesc.Draw.Z)
                    };
            }
            Random random = new Random((int)seeds);
            return
                drawDesc with
                {
                    Draw = new(
                    drawDesc.Draw.X + random.Next(Math.Abs((int)x)) -50,
                    drawDesc.Draw.Y + random.Next(Math.Abs((int)y)) - 50,
                    drawDesc.Draw.Z)
                };
        }
        public void ClearInput()
        {
            input = null;
        }
        public void SetInput(ID2D1Image? input)
        {
            this.input = input;
        }

        public void Dispose()
        {

        }

    }
}