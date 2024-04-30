using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiFiSy_TPI.GameElement.Firework
{
    public interface IFirework
    {
        Vector2 StartPosition { get; set; }
        float Lifespan { get; set; }
        float LaunchTime { get; set; }
        float StartSpeed { get; set; }
        void Update();
    }
}
