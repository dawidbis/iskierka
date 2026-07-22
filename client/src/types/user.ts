export type User = {
  id:           string;
  displayName:  string;
  emailAddress: string;
  token:        string;
  imageUrl?:    string;
}

export type LoginCreds = {
  emailAddress: string;
  password:     string;
}

export type RegisterCreds = {
  displayName:  string;
  emailAddress: string;
  password:     string;
}
