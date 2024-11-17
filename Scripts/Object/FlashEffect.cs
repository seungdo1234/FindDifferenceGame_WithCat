using Cysharp.Threading.Tasks;
using UnityEngine;
public class FlashEffect : PoolObject
{
    protected ParticleSystem particleSystem;
    
    public void EffectInit(E_PoolObjectType type ,Vector3 pos)
    {
        transform.position = pos;

        if (!particleSystem)
            particleSystem = GetComponent<ParticleSystem>();
        
        particleSystem.Play();
        DeActivateEffect(type).Forget();
    }

    protected async UniTaskVoid DeActivateEffect(E_PoolObjectType type)
    {
        await UniTask.WaitUntil(() => !particleSystem.IsAlive());
        
        PoolManager.Instance.ReturnToPool(type, this);
    }
}
