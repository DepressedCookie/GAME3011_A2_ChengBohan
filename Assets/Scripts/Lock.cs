using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    [SerializeField] float ResetTime = 10;
    public LockButton[] Pins;
    public bool Unlocked = false;
    public int CurrentOrder = 1;
    bool Started = false;
    // Start is called before the first frame update
    void Start()
    {
        AssignOrder();
        StartCoroutine(ResetPins());
    }

    IEnumerator ResetPins()
    {
        while (true)
        {
            if (Started)
            {
                yield return new WaitForSeconds(ResetTime);
                ResetLock();
            }
            yield return false;
        }
    }

    void ResetLock(int order)
    {
        if (!Unlocked)
        {
            foreach (LockButton pin in Pins)
            {
                if (pin.Order != order)
                {
                    pin.ResetPin();
                }
            }
            CurrentOrder = 1;
        }
    }

    void ResetLock()
    {
        if (!Unlocked)
        {
            foreach (LockButton pin in Pins)
            {
                pin.ResetPin();
            }
            Started = false;
            CurrentOrder = 1;
        }
    }

    void AssignOrder()
    {
        for (int i = 0; i < 5; i++)
        {
            int RandOrder = Random.Range(1, 6);
            if(!hasOrder(RandOrder))
            {
                Pins[i].Order = RandOrder;
                if (FindObjectOfType<GameMana>().Difficulty > 0)
                {
                    Pins[RandOrder - 1].shouldHint = false;
                    FindObjectOfType<GameMana>().Difficulty--;
                }
            }
            else
            {
                i--;
            }
        }
    }

    bool hasOrder(int order)
    {
        foreach (LockButton pin in Pins)
        {
            if(pin.Order == order)
            {
                return true;
            }
        }
        return false;
    }

    bool DoneTry()
    {
        foreach (LockButton pin in Pins)
        {
            if(!pin.Unlocked)
            {
                return false;
            }
        }
        return true;
    }

    public void TryPin(int order)
    {
        Started = true;
        if(order == CurrentOrder)
        {
            CurrentOrder++;
        }
        else
        {
            ResetLock(order);
        }
        if(CurrentOrder == 6)
        {
            Unlocked = true;
        }
        if (DoneTry())
        {
            ResetLock();
        }
    }
}
