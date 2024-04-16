using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBox : MonoBehaviour
{
    public float range = 20f;
    public float verticalRange = 20f;

    public float weaponNoiseRadius = 20f;

    public float fireRate = 1;
    private float nextTimeToFire;
    public int maxAmmo;
    public int ammo;

    public Animator weaponSpriteAnimator;

    public float smallDamage = 1f;
    public float bigDamage = 2f;

    public LayerMask enemyLayerMask;
    public LayerMask raycastLayerMask;

    private BoxCollider weaponTrigger;
    public EnemiesInRange enemiesInRange;

    // Start is called before the first frame update
    void Start()
    {
        weaponTrigger = GetComponent<BoxCollider>();
        weaponTrigger.size = new Vector3(1, verticalRange, range);
        weaponTrigger.center = new Vector3(0, 0, range * 0.5f);

        ammo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > nextTimeToFire && ammo > 0){
            Fire();
        }
        if (ammo == 0 && Input.GetKeyDown(KeyCode.R)){
            Reload();
        }
    }

    void Fire(){
        // simulate weapon noise radius
        Collider[] enemyColliders;
        enemyColliders = Physics.OverlapSphere(transform.position, weaponNoiseRadius, enemyLayerMask);

        // alert any enemy in range based on noise
        foreach (var enemyCollider in enemyColliders){
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy.isActive){
                enemy.isAggro = true;
            }
        }

        weaponSpriteAnimator.SetTrigger("Fire");

        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().Play();

        foreach (var enemy in enemiesInRange.enemiesInRangeList){
            var dir = enemy.transform.position - transform.position;

            RaycastHit hit;
            // raycastLayerMask checks for default and enemy, in other words everything but the weapon
            if (Physics.Raycast(transform.position, dir, out hit, range * 1.5f, raycastLayerMask)){
                if (hit.transform == enemy.transform){
                    float dist = Vector3.Distance(enemy.transform.position, transform.position);

                    if (dist > range * 0.5f){
                        enemy.TakeDamage(smallDamage);
                    }else{
                        enemy.TakeDamage(bigDamage);
                    }
                }
            }
        }

        // reset nextTimeToFire
        nextTimeToFire = Time.time + fireRate;
        ammo--;
    }

    void Reload(){
        weaponSpriteAnimator.SetTrigger("Reload");
        ammo = maxAmmo;
    }

    public void GiveAmmo(int amount, GameObject pickup){
        if (ammo < maxAmmo){
            ammo += amount;
            Destroy(pickup);
        }

        if (ammo > maxAmmo){
            ammo = maxAmmo;
        }
    }

    void OnTriggerEnter(Collider other){
        // add potential enemy to shoot
        Enemy enemy = other.transform.GetComponent<Enemy>();

        if (enemy){
            enemiesInRange.AddEnemy(enemy);
        }
    }

    void OnTriggerExit(Collider other){
        Enemy enemy = other.transform.GetComponent<Enemy>();

        if (enemy){
            enemiesInRange.RemoveEnemy(enemy);
        }
    }
}
