CREATE TABLE zipuser (
  id                    bigint                                      NOT NULL,
  emailaddress          VARCHAR(100)                                NOT NULL    UNIQUE,  
  name                  VARCHAR(500)                                NOT NULL,
  monthlysalary         NUMERIC                         DEFAULT 0   NOT NULL    CHECK   (monthlysalary   >= 0),
  monthlyexpenses       NUMERIC                         DEFAULT 0   NOT NULL    CHECK   (monthlyexpenses >= 0),

  CONSTRAINT user_pkey  PRIMARY KEY (id)
);

CREATE TABLE account (
  id                    bigint                                          NOT NULL,
  name                  VARCHAR(500)                                    NOT NULL,
  createdbyuserid       bigint                                          NOT NULL,

  CONSTRAINT account_pkey PRIMARY KEY(id)
);

CREATE INDEX createdbyuserid_idx ON account
  USING btree (createdbyuserid);