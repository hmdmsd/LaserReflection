using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class LaserBeam
{
    UnityEngine.Vector3 pos,dir;
    GameObject laserObj ;
    LineRenderer laser;
    List<UnityEngine.Vector3> laserIndices = new List<UnityEngine.Vector3>();

    public LaserBeam(UnityEngine.Vector3 pos, UnityEngine.Vector3 dir , Material material){
        this.laser = new LineRenderer();
        this.laserObj = new GameObject();
        this.laserObj.name = "Laser Beam";
        this.pos = pos;
        this.dir = dir ;

        this.laser = this.laserObj.AddComponent(typeof(LineRenderer)) as LineRenderer ;
        this.laser.startWidth = 0.1f;
        this.laser.endWidth = 0.1f;
        this.laser.material = material;
        this.laser.startColor = Color.red;
        this.laser.endColor = Color.red;

        CastRay(pos,dir,laser);
    }

    void CastRay(UnityEngine.Vector3 pos , UnityEngine.Vector3 dir , LineRenderer laser){
        laserIndices.Add(pos);
        
        Ray ray = new Ray(pos,dir);
        RaycastHit hit;

        if ( Physics.Raycast(ray, out hit , 30 , 1))
        {
            CheckHit(hit,dir,laser);
        }
        else{
            laserIndices.Add(ray.GetPoint(30));
            UpdateLaser();
        }
    }

    void UpdateLaser(){
        int count = 0;
        laser.positionCount = laserIndices.Count;
        foreach(UnityEngine.Vector3 index in laserIndices){
            laser.SetPosition(count,index);
            count++;
        }
    }

    void CheckHit(RaycastHit hitInfo , UnityEngine.Vector3 direction , LineRenderer laser){
        if(hitInfo.collider.gameObject.tag == "Mirror"){
            UnityEngine.Vector3 pos = hitInfo.point ;
            UnityEngine.Vector3 dir = UnityEngine.Vector3.Reflect(direction,hitInfo.normal);
            CastRay(pos,dir,laser);
        }
        else{
            laserIndices.Add(hitInfo.point);
            UpdateLaser();
        }
    }

}
