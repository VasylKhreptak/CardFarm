using CBA.Actions.Core;
using UnityEngine.SceneManagement;

namespace CBA.Actions.Management.Scene
{
    public class ReloadScene : Action
    {
        public override void Do()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
