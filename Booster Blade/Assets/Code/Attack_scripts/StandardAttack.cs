using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class StandardAttack : ScriptableObject
{

    [SerializeField]
    [Tooltip("Interval between shots. Use lower values for a faster rate.")]
    protected float rateOfFire;

    [SerializeField]
    [Tooltip("Bullet used in attack with single bullet types")]
    protected GameObject bulletPrefab;

    [SerializeField]
    private string fireSFX = "Shoot1";

    public List<BulletKeyPoint> bulletKeyPoints;

    private Vector2 laserSize = new Vector2(1, 1);
    public Color bulletColor = Color.white;
    private bool hasLooseLaser = false;

    //sound effect string that can be looked at?

    //i considered making this static but i realized that didn't make sense
    //return bullet?
    public void ShootBulletOld(GameObject bul, float bulSpeed) //use this instead?
    {

        if (hasLooseLaser)
        {
            LooseLaser looseLaser = bul.GetComponent<LooseLaser>();
            looseLaser.fullSize = laserSize;
        }
        bul.GetComponent<BulletMovement>().SetInitialVelocity(bulSpeed);
        bul.GetComponent<BulletBehavior>().bulletKeyPoints = bulletKeyPoints;
        bul.GetComponent<BulletBehavior>().SetBulletColor(bulletColor);

        bul.SetActive(true);
    }


    public void ShootBullet(GameObject bul, Vector2 bulPosition, float bulAngle, float bulSpeed) //use this instead?
    {
        if (hasLooseLaser)
        {
            LooseLaser looseLaser = bul.GetComponent<LooseLaser>();
            looseLaser.fullSize = laserSize;
        }
        bul.GetComponent<BulletMovement>().SetInitialVelocity(bulSpeed);
        bul.GetComponent<BulletBehavior>().bulletKeyPoints = bulletKeyPoints;
        bul.GetComponent<BulletBehavior>().SetBulletColor(bulletColor);

        bul.transform.position = bulPosition;
        bul.transform.rotation = Quaternion.Euler(0, 0, bulAngle);
        bul.SetActive(true);
    }
    //only use with standard bullets and lasers?

    public void TriggerAttack()
    {
        //does different things based on attack type?
    }
    public virtual float GetRateOfFire()
    {
        return rateOfFire;
    }
    public void PlayAttackSFX()
    {
        if (!fireSFX.Equals(""))
        {
            AudioManager.instance.Play(fireSFX);
        }

    }

    #region Editor
#if UNITY_EDITOR
    //...why is GetRateOfFire() in this section?

    [CustomEditor(typeof(StandardAttack), true)]
    public class StandardAttackEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI(); //draws default items
            StandardAttack standardAttack = (StandardAttack)target;
            EditorGUILayout.LabelField("Bullet Details");
            //Serialize after
            //CHECK IF BULLET EVEN EXISTS FIRST
            if (standardAttack.bulletPrefab != null)
            {
                if (standardAttack.bulletPrefab.GetComponent<LooseLaser>() != null)
                {
                    standardAttack.hasLooseLaser = true;
                    EditorGUILayout.LabelField("Laser Size");
                    standardAttack.laserSize = EditorGUILayout.Vector2Field("Laser Size", standardAttack.laserSize);
                }
                else
                {
                    standardAttack.hasLooseLaser = false;
                }

            }
        }
    }

#endif
    #endregion
}
