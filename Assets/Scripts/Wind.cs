using UnityEngine;
using Random = UnityEngine.Random;

public class Wind : MonoBehaviour
{
    // public CommandInterface ci;
    public AnimationCurve rotCurveX;
    public AnimationCurve antiRotCurveX;
    public AnimationCurve rotCurveZ;
    public AnimationCurve antiRotCurveZ;
    private Rigidbody _rb;
    private float speed = 4f;
    private float rdm;
    private ParticleSystem windPS;
    // Start is called before the first frame update
    private void Start()
    {
        windPS = GetComponent<ParticleSystem>();
        rdm = Random.Range(1f, 2f);
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = speed * transform.forward;
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Plank"))
        {
            var i = Vector3.Dot(GetComponent<Rigidbody>().velocity.normalized, other.transform.right);
            var right = other.transform.right;
            if (i > 0.1 && i < 0.9)
            {
                _rb.velocity = right * speed;
                transform.forward = right;
                transform.position = other.transform.position + new Vector3(0.15f, 0f, 0f);
            }
            else if (i < -0.1 && i > -0.9)
            {
                _rb.velocity = -right * speed;
                transform.forward = -right;
                transform.position = other.transform.position - new Vector3(0.15f, 0f, 0f);
            }
            else if (i > -0.1 && i < 0.1)
            {
                //other.GetComponent<Collider>().enabled = false;
                _rb.velocity = Vector3.zero;
                var vel = windPS.velocityOverLifetime;
                vel.x = new ParticleSystem.MinMaxCurve(1, antiRotCurveX, rotCurveX);
                vel.z = new ParticleSystem.MinMaxCurve(1, antiRotCurveZ, rotCurveZ);
                var main = windPS.main;
                //main.startLifetimeMultiplier = 0.2f;
                main.loop = false;
                Destroy(gameObject, 5f);
            }
        }
        else if (other.gameObject.CompareTag("Wind"))
        {
            var i = Vector3.Dot(gameObject.GetComponent<Rigidbody>().velocity.normalized, other.transform.forward);
            //other.GetComponent<Collider>().enabled = false;
            //transform.position = other.transform.position;
            if (i < -0.9 && i > -1.1)
            {
                _rb.velocity = Vector3.zero;
                var vel = windPS.velocityOverLifetime;
                vel.x = new ParticleSystem.MinMaxCurve(1, antiRotCurveX, rotCurveX);
                vel.z = new ParticleSystem.MinMaxCurve(1, antiRotCurveZ, rotCurveZ);
                var main = windPS.main;
                //main.startLifetimeMultiplier = 0.2f;
                main.loop = false;
                Destroy(gameObject, 5f);
            }
        }
        /*
        else if (other.CompareTag("Vane"))
        {
            if (other.transform.forward == transform.forward) return;
            var rc = new RotateCommand(other.transform, transform.forward);
            rc.Execute();
            ci.command.affected.Push(rc);
        }
        */
        else if (other.gameObject.CompareTag("DeadZone"))
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Barrier"))
        {
            _rb.velocity = Vector3.zero;
            var vel = windPS.velocityOverLifetime;
            vel.x = new ParticleSystem.MinMaxCurve(1, antiRotCurveX, rotCurveX);
            vel.z = new ParticleSystem.MinMaxCurve(1, antiRotCurveZ, rotCurveZ);
            var main = windPS.main;
            //main.startLifetimeMultiplier = 0.2f;
            main.loop = false;
            Destroy(gameObject, 5f);
        }
    }
}
