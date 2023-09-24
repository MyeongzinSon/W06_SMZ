using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{

	private float timer;
	[SerializeField] private float destroyMissileTime;
	[SerializeField] private float rotateSpeed;
	[SerializeField] private float guideSpeed;
	private Rigidbody2D rigid;
	private GameObject player;

    private void Start()
    {
		player = GameManager.instance.GetPlayer();
		rigid = GetComponent<Rigidbody2D>();
    }
    private void GuideToPlayer()
	{
		if (timer <= destroyMissileTime)
		{
			Vector2 dir = transform.right;
			Vector2 targetDir = player.transform.position - transform.position;
			Vector3 crossVec = Vector3.Cross(dir, targetDir);
			float inner = Vector3.Dot(Vector3.forward, crossVec);
			float saveAngle = inner + transform.rotation.eulerAngles.z;
			transform.rotation = Quaternion.Euler(0, 0, saveAngle * rotateSpeed);

			float moveDirAngle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
			Vector2 moveDir = Vector2.zero;
			moveDir = new Vector2(Mathf.Cos(moveDirAngle), Mathf.Sin(moveDirAngle));
			rigid.velocity = moveDir * guideSpeed;
			timer += Time.deltaTime;
		}
		else
		{
			Destroy(gameObject);
		}
	}

    private void Update()
    {
		GuideToPlayer();
    }

}
