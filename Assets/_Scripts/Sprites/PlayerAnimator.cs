using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator Anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Attack()
    {
        Anim.SetTrigger("AttackSpell");
    }
    public void Shield()
    {
        Anim.SetTrigger("ShieldSpell");
    }
}
