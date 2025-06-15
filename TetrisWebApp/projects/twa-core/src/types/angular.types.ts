export type NgClassInput = string | string[] | Set<string> | { [cssClass: string]: any } | null | undefined;

export type PartiallyRequired<TValue, TRequired extends keyof TValue> = Partial<TValue> &
   Required<Pick<TValue, TRequired>>;
